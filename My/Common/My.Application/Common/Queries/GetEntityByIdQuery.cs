using My.CrossCuttingConcerns.Exceptions;
using My.Domain.Entities;
using My.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace My.Application;

public class GetEntityByIdQuery<TEntity> : IQuery<TEntity>
    where TEntity : Entity<Guid>, IAggregateRoot
{
    public Guid Id { get; set; }
    public bool ThrowNotFoundIfNull { get; set; }
}

internal class GetEntityByIdQueryHandler<TEntity> : IQueryHandler<GetEntityByIdQuery<TEntity>, TEntity>
where TEntity : Entity<Guid>, IAggregateRoot
{
    private readonly IRepository<TEntity, Guid> _repository;

    public GetEntityByIdQueryHandler(IRepository<TEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<TEntity> HandleAsync(GetEntityByIdQuery<TEntity> query, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.FirstOrDefaultAsync(_repository.GetQueryableSet().Where(x => x.Id == query.Id));

        if (query.ThrowNotFoundIfNull && entity == null)
        {
            throw new NotFoundException($"Entity {query.Id} not found.");
        }

        return entity;
    }
}
