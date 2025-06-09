using Accidenta.Application.DTO;
using Accidenta.Domain.Interfaces;

namespace Accidenta.Application.Accounts.Specifications
{
    public class CreateAccountSpecification : ISpecification<CreateAccountRequest>
    {
        public string ErrorMessage { get; private set; } = string.Empty;

        public bool IsSatisfiedBy(CreateAccountRequest request)
        {
            if (request.Contact == null)
            {
                ErrorMessage = "Contact information must be provided.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.Contact.Email))
            {
                ErrorMessage = "Contact email must not be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.Contact.FirstName) || string.IsNullOrWhiteSpace(request.Contact.LastName))
            {
                ErrorMessage = "Contact first name and last name must not be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.AccountName))
            {
                ErrorMessage = "Account name must not be empty.";
                return false;
            }

            return true;
        }
    }
}
