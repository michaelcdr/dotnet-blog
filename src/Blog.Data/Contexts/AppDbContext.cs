using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().HasKey(c => c.Id);

        modelBuilder.Entity<Categoria>()
            .HasMany(e => e.Posts)
            .WithOne(e => e.Categoria);

        modelBuilder.Entity<Post>().HasKey(c => c.Id);

        modelBuilder.Entity<Post>()
            .HasOne(e => e.Categoria)
            .WithMany(e => e.Posts)
            .HasForeignKey(e => e.CategoriaId);

        base.OnModelCreating(modelBuilder);
    }
}
