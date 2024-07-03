using My.Domain.Entities;
using System;

namespace My.Services.Identity.Entities;

public class RoleClaim : Entity<Guid>
{
    public string Type { get; set; }
    public string Value { get; set; }

    public Role Role { get; set; }
}
