using ContactsWeb.Data;
using ContactsWeb.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContactsWeb.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public List<Contact> Contacts { get; set; } = new();

    public IndexModel(AppDbContext db) => _db = db;

    public async Task OnGetAsync()
    {
        Contacts = await _db.Contacts.AsNoTracking().ToListAsync();
    }
}
