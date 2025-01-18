using Blog.Core.Data;
using Blog.Core.Messages;
using Blog.Posts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Posts.Data.Contexts.SQLite;

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Post> Posts { get; set; }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().HasKey(c => c.Id);

        modelBuilder.Entity<Categoria>()
            .HasMany(e => e.Posts)
            .WithOne(e => e.Categoria);

        modelBuilder.Entity<Post>().HasKey(c => c.Id);

        modelBuilder.Entity<Post>().Property(c => c.Imagem).IsRequired(false);

        modelBuilder.Entity<Post>()
            .HasOne(e => e.Categoria)
            .WithMany(e => e.Posts)
            .HasForeignKey(e => e.CategoriaId);


        modelBuilder.Ignore<Event>();

        base.OnModelCreating(modelBuilder);
    }
}
