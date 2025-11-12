using ContactsWeb.Data;
using ContactsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactsWeb.Pages;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _db;
    [BindProperty] public Contact Contact { get; set; } = new();

    public DeleteModel(AppDbContext db) => _db = db;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var c = await _db.Contacts.FindAsync(id);
        if (c == null) return RedirectToPage("Index");
        Contact = c;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact != null)
        {
            _db.Contacts.Remove(contact);
            await _db.SaveChangesAsync();
        }
        return RedirectToPage("Index");
    }
}
