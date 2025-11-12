using ContactsWeb.Data;
using ContactsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactsWeb.Pages;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    [BindProperty] public Contact Contact { get; set; } = new();

    public EditModel(AppDbContext db) => _db = db;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Contact = await _db.Contacts.FindAsync(id) ?? new();
        if (Contact.Id == 0) return RedirectToPage("Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        _db.Attach(Contact).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
