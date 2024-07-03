using My.CrossCuttingConcerns.DateTimes;
using My.Domain.Entities;
using My.Infrastructure.Persistence;

namespace My.Services.Identity.Repositories;

public class Repository<T, TKey> : DbContextRepository<IdentityDbContext, T, TKey>
    where T : Entity<TKey>, IAggregateRoot
{
    public Repository(IdentityDbContext dbContext, IDateTimeProvider dateTimeProvider)
        : base(dbContext, dateTimeProvider)
    {
    }
}
