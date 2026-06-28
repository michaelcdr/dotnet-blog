using Blog.Auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Auth.Data;

public class AuthContext : IdentityDbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

    public DbSet<AuthClientRefreshToken> ClientRefreshTokens => Set<AuthClientRefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AuthClientRefreshToken>(entity =>
        {
            entity.ToTable("AuthClientRefreshTokens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClientId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.TokenHash).HasMaxLength(128).IsRequired();
            entity.Property(e => e.ReplacedByTokenHash).HasMaxLength(128);
            entity.Ignore(e => e.IsActive);
            entity.HasIndex(e => e.TokenHash).IsUnique();
            entity.HasIndex(e => e.ClientId);
        });
    }
}