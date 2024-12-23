using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Context;

public class MerContext(
    DbContextOptions<MerContext> options
) : IdentityDbContext<IdentityUser>(options)
{
    //public reqired DbSet<Table> Tables{get;set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var encryptionConverter = new Services.EncryptionConverter();

        // Nombre de entidadews a minúsculas, porque MySQL en Linux así trabaja: SHOW VARIABLES LIKE 'lower_case_table_names';
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName == null) continue;
            entity.SetTableName(tableName.ToLower());
        }
    }
}