using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ContactsApi.Data;
using ContactsApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Host=postgres;Port=5432;Database=contactsdb;Username=postgres;Password=postgres";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Allow simple CORS for demo (nginx + browser)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Initialize DB (dev/demo). For production use migrations.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Ensure DB exists - for demo/CI. Replace with Migrate() in real deployments with migrations.
    db.Database.EnsureCreated();
}

// Middlewares
app.UseCors();
app.MapGet("/", () => Results.Ok(new { Message = "Contacts API running" }));

app.MapGet("/contacts", async (AppDbContext db) =>
{
    var list = await db.Contacts.AsNoTracking().ToListAsync();
    return Results.Ok(list);
});

app.MapGet("/contacts/{id:int}", async (int id, AppDbContext db) =>
{
    var contact = await db.Contacts.FindAsync(id);
    return contact is not null ? Results.Ok(contact) : Results.NotFound();
});

app.MapPost("/contacts", async (CreateContactDto dto, AppDbContext db) =>
{
    var contact = new Contact { Name = dto.Name.Trim(), Email = dto.Email.Trim() };
    db.Contacts.Add(contact);
    await db.SaveChangesAsync();
    return Results.Created($"/contacts/{contact.Id}", contact);
});

app.MapPut("/contacts/{id:int}", async (int id, UpdateContactDto dto, AppDbContext db) =>
{
    var contact = await db.Contacts.FindAsync(id);
    if (contact is null) return Results.NotFound();

    contact.Name = dto.Name.Trim();
    contact.Email = dto.Email.Trim();
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/contacts/{id:int}", async (int id, AppDbContext db) =>
{
    var contact = await db.Contacts.FindAsync(id);
    if (contact is null) return Results.NotFound();

    db.Contacts.Remove(contact);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

record CreateContactDto([Required][StringLength(200)] string Name, [Required][EmailAddress] string Email);
record UpdateContactDto([Required][StringLength(200)] string Name, [Required][EmailAddress] string Email);
