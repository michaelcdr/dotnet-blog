using Microsoft.EntityFrameworkCore;

namespace CodingBlog.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.Categoria);

            modelBuilder.Entity<Post>()
                .HasOne(e => e.Categoria)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.CategoriaId);

            base.OnModelCreating(modelBuilder);
        }
    }
} 