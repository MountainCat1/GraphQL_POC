using QuickShop.Application.Errors;
using Catut.Infrastructure.Abstractions;
using Catut.Infrastructure.Errors.Database;

namespace QuickShop.Application.Services;


public class DatabaseErrorMapper : IDatabaseErrorMapper
{
    public Task<Exception> MapAsync(DatabaseException exception)
    {
        if (exception is ItemNotFoundException)
            return Task.FromResult<Exception>(new NotFoundError(null, exception));

        return Task.FromResult<Exception>(exception);
    }
}