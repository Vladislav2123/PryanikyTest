using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PryanikyTest.Domain;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Application.Abstractions;

public interface IApplicationDbContext
{
    public DatabaseFacade Database { get; }

    // DbSet<AppUser> Users { get; set; }
    DbSet<Seller> Sellers { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<ProductOrder> ProductOrders { get; set; }
    DbSet<Order> Orders { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
