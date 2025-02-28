using GameStore.Api.Entities;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext:DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options):base(options)
    {
                
    }

    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
}