using api.Common;
using api.Context;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public interface INameService<TEntity> where TEntity : class, INameEntity
{
    Task<ICollection<TEntity>> GetAsync(string userId);
    Task CreateAsync(string userId, string name);
    Task UpdateAsync(string userId, short id, string name);
}

public class NameService<TEntity>(
    MerContext context,
    ILogService logs,
    ICommonService functions
) : INameService<TEntity> where TEntity : class, INameEntity, new()
{
    private readonly MerContext _context = context;
    private readonly ILogService _logs = logs;
    private readonly ICommonService _functions = functions;
    private readonly DbSet<TEntity> _entity = context.Set<TEntity>();

    private async Task AlreadyExists(
        string name
    )
    {
        var actions = await _entity.AsNoTracking().ToListAsync();
        actions.ForEach(a => a.Name = _functions.GetNormalizedText(a.Name));
        var alreadyExists = actions.Any(a => a.Name == _functions.GetNormalizedText(name));
        if (alreadyExists) throw new AlreadyExistsException("Ya existe este registro");
    }

    public async Task CreateAsync(
        string userId,
        string name
    )
    {
        var nName = name;
        await AlreadyExists(nName);
        var record = new TEntity
        {
            Name = name
        };
        _entity.Add(record);
        await _context.SaveChangesAsync();
        var table = _context.Model.FindEntityType(typeof(TEntity));
        await _logs.RegisterCreationAsync(userId, $"{table} {record.Id}");
    }

    public async Task<ICollection<TEntity>> GetAsync(
        string userId
    )
    {
        var table = _context.Model.FindEntityType(typeof(TEntity));
        await _logs.RegisterReadingAsync(userId, $"Todos de {table}");
        return await _entity.ToListAsync();
    }

    public async Task UpdateAsync(
        string userId,
        short id,
        string name
    )
    {
        var record = await _entity.FindAsync(id) ?? throw new DoesNotExistsException("El registro no existe");
        if (_functions.GetNormalizedText(record.Name) == _functions.GetNormalizedText(name)) return;
        await AlreadyExists(name);
        record.Name = name;
        await _context.SaveChangesAsync();
        var table = _context.Model.FindEntityType(typeof(TEntity));
        await _logs.RegisterUpdateAsync(userId, $"{table} {id}");
    }
}