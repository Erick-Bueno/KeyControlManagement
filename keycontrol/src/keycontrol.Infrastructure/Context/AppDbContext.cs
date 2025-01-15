using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace keycontrol.Infrastructure.Context;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> users { get; set; }
    public DbSet<KeyRoom> keys { get; set; }
    public DbSet<Report> reports { get; set; }
    public DbSet<Token> tokens { get; set; }
    public DbSet<Room> rooms { get; set; }
    public DbSet<Role> roles { get; set; }
    public DbSet<RolePermission> rolePermissions { get; set; }
    public DbSet<Permission> permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!AppDbContextFactory.IsMigrationContext)
        {
            var permissionsEnum = Enum.GetValues<Domain.Enums.Permission>()
            .Where(p => p != Domain.Enums.Permission.None)
            .Select(p => new Permission { Id = (int)p, Name = p.ToString() });
            var rolePermissions = new[]
            {
            new RolePermission() { RoleId = Role.Doorman.Id, PermissionId = (int)Domain.Enums.Permission.Administrator },
            new RolePermission() { RoleId = Role.CommonUser.Id, PermissionId = (int)Domain.Enums.Permission.ReadMember }
        };

            optionsBuilder
                .UseSeeding((context, _) => {
                    context.Set<Permission>()
                        .ExecuteDelete();
                    context.Set<Role>()
                        .ExecuteDelete();
                    context.Set<RolePermission>()
                        .ExecuteDelete();

                    context.Set<Permission>().AddRange(permissionsEnum);
                    context.Set<Role>().AddRange(Role.GetValues());
                    context.Set<RolePermission>().AddRange(rolePermissions);

                    context.SaveChanges();
                })
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    await context.Set<Permission>()
                        .ExecuteDeleteAsync(cancellationToken);
                    await context.Set<Role>()
                        .ExecuteDeleteAsync(cancellationToken);
                    await context.Set<RolePermission>()
                        .ExecuteDeleteAsync(cancellationToken);

                    context.Set<Permission>().AddRange(permissionsEnum);
                    context.Set<Role>().AddRange(Role.GetValues());
                    context.Set<RolePermission>().AddRange(rolePermissions);

                    await context.SaveChangesAsync(cancellationToken);
                });
        }

    }
}