using Accidenta.Domain.Entities;

namespace Accidenta.Application.Contacts.DTO;

public class ContactDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;

    public ContactDto(Contact contact)
    {
        Id = contact.Id;
        FirstName = contact.FirstName;
        LastName = contact.LastName;
        Email = contact.Email;
    }

    public ContactDto() { }
}
