using keycontrol.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace keycontrol.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> users { get; set; }
    public DbSet<Key> keys { get; set; }
    public DbSet<Report> reports { get; set; }
    public DbSet<Token> tokens { get; set; }
    public DbSet<Room> rooms { get; set; }
}