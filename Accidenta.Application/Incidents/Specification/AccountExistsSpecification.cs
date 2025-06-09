using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Incidents.Specification
{
    public class AccountExistsSpecification : ISpecification<Account>
    {
        public string ErrorMessage => "Account must exist to create an incident.";

        public bool IsSatisfiedBy(Account? account)
        {
            return account != null;
        }
    }
}
