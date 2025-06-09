using Accidenta.Application.Contacts.DTO;
using Accidenta.Domain.Entities;

namespace Accidenta.Application.Accounts.DTO;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ContactDto? Contact { get; set; }

    public AccountDto(Account account)
    {
        Id = account.Id;
        Name = account.Name;
        Contact = account.Contact != null ? new ContactDto(account.Contact) : null;
    }
}
