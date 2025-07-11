﻿namespace Accidenta.Domain.Entities;

public class Contact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!; // Unique
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
