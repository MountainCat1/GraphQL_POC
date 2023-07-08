using QuickShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace QuickShop.Infrastructure.Contexts;

public class QuickShopDbContext : DbContext
{
    public SomeEntity SomeEntity { get; set; }
    
    public QuickShopDbContext(DbContextOptions<QuickShopDbContext> options) : base(options)
    {
        
    }
}