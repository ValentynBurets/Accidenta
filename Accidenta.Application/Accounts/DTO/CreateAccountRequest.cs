using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.DTO
{
    public class CreateAccountRequest
    {
        public string AccountName { get; set; } = null!;
        public CreateContactRequest Contact { get; set; } = null!;
    }
}
