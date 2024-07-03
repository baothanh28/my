using My.Domain.Entities;
using System;

namespace My.Services.Identity.Entities;

public class UserToken : Entity<Guid>
{
    public Guid UserId { get; set; }

    public string LoginProvider { get; set; }

    public string TokenName { get; set; }

    public string TokenValue { get; set; }
}
