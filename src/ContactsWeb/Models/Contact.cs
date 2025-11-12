using System.ComponentModel.DataAnnotations;

namespace ContactsWeb.Models;

public class Contact
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Name { get; set; } = default!;

    [Required, EmailAddress]
    public string Email { get; set; } = default!;
}
