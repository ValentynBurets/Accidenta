namespace Accidenta.Application.DTO;

public class CreateIncidentRequest
{
    public string AccountName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Description { get; set; } = null!;
}
