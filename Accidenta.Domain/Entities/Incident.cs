namespace Accidenta.Domain.Entities;

public class Incident
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Description { get; set; } = null!;
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
}
