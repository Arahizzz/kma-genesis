using DataService.Models;
using Microsoft.EntityFrameworkCore;

namespace DataService;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public UserContext(DbContextOptions options) : base(options)
    {
    }

    public async Task AddUser(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();
    }
}