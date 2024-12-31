using api.Context;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface IPublicService
{
    Task TestDBAsync();
}

public class PublicService(
    MerContext context
) : IPublicService
{
    private readonly MerContext _context = context;
    public async Task TestDBAsync()
    {
        await _context.Database.OpenConnectionAsync();
        await _context.Database.CloseConnectionAsync();
    }
}