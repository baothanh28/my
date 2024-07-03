using My.Application;
using My.Services.Identity.Entities;
using My.Services.Identity.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace My.Services.Identity.Queries;

public class GetUserQuery : IQuery<User>
{
    public Guid Id { get; set; }
    public bool IncludeClaims { get; set; }
    public bool IncludeUserRoles { get; set; }
    public bool IncludeRoles { get; set; }
    public bool AsNoTracking { get; set; }
}

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> HandleAsync(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        var db = _userRepository.Get(new UserQueryOptions
        {
            IncludeClaims = query.IncludeClaims,
            IncludeUserRoles = query.IncludeUserRoles,
            IncludeRoles = query.IncludeRoles,
            AsNoTracking = query.AsNoTracking,
        });

        return _userRepository.FirstOrDefaultAsync(db.Where(x => x.Id == query.Id));
    }
}
