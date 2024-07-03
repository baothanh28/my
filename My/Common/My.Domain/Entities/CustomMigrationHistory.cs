using System;

namespace My.Domain.Entities;

public class CustomMigrationHistory : Entity<Guid>
{
    public string MigrationName { get; set; }
}
