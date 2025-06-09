namespace Accidenta.Application.DTO;

public class CreateAccountRequest
{
    public string AccountName { get; set; } = null!;
    public CreateContactRequest Contact { get; set; } = null!;
}
