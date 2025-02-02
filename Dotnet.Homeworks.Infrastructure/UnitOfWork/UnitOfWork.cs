﻿using Dotnet.Homeworks.Data.DatabaseContext;

namespace Dotnet.Homeworks.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}