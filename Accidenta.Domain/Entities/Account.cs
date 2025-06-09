namespace Accidenta.Domain.Entities;

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!; // Unique
    public Guid ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
