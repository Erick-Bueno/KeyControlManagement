using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace keycontrol.Infrastructure.Context;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
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
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TokenConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new KeyConfiguration());
        modelBuilder.ApplyConfiguration(new ReportConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var permissionsEnum = Enum.GetValues<Domain.Enums.Permission>()
            .Where(p => p != Domain.Enums.Permission.None)
            .Select(p => new Permission { Id = (int)p, Name = p.ToString() });
        var rolePermissions = new[]
        {
            new RolePermission() { RoleId = Role.Doorman.Id, PermissionId = (int)Domain.Enums.Permission.Administrator },
            new RolePermission() { RoleId = Role.CommonUser.Id, PermissionId = (int)Domain.Enums.Permission.ReadMember }
        };

        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Default"))
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

                await context.Set<Permission>().AddRangeAsync(permissionsEnum, cancellationToken);
                await context.Set<Role>().AddRangeAsync(Role.GetValues(), cancellationToken);
                await context.Set<RolePermission>().AddRangeAsync(rolePermissions, cancellationToken);
                
                await context.SaveChangesAsync(cancellationToken);
            });
        
    }
}