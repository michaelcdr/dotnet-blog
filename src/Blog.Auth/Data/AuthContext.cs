using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Auth.Data;

public class AuthContext : IdentityDbContext
{
    public AuthContext(DbContextOptions<AuthContext> options):base(options) { }
}
