using Accidenta.Application.Contacts.DTO;
using Accidenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Accounts.DTO
{
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
}
