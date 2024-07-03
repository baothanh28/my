﻿using My.CrossCuttingConcerns.DateTimes;
using My.Services.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace My.Services.Identity.Repositories;

public class RoleRepository : Repository<Role, Guid>, IRoleRepository
{
    public RoleRepository(IdentityDbContext dbContext, IDateTimeProvider dateTimeProvider)
        : base(dbContext, dateTimeProvider)
    {
    }

    public IQueryable<Role> Get(RoleQueryOptions queryOptions)
    {
        var query = GetQueryableSet();

        if (queryOptions.IncludeClaims)
        {
            query = query.Include(x => x.Claims);
        }

        if (queryOptions.IncludeUserRoles)
        {
            //query = query.Include(x => x.UserRoles);
        }

        if (queryOptions.IncludeUsers)
        {
            query = query.Include("UserRoles.User");
        }

        if (queryOptions.AsNoTracking)
        {
            query = query = query.AsNoTracking();
        }

        return query;
    }
}
