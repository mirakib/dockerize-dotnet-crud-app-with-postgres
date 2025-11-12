using ContactsWeb.Data;
using ContactsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactsWeb.Pages;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    [BindProperty] public Contact Contact { get; set; } = new();

    public CreateModel(AppDbContext db) => _db = db;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.Contacts.Add(Contact);
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
