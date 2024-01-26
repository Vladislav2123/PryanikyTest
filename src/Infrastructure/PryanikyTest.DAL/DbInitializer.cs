using PryanikyTest.Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PryanikyTest.Domain.Entities;
using MediatR;

namespace PryanikyTest.DAL;

/// <summary>
/// Initializes Database
/// </summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(
		IApplicationDbContext dbContext,
		IMediator mediator,
		IWebHostEnvironment environment)
	{
		if (environment.IsDevelopment())
			dbContext.Database.EnsureDeleted();

		dbContext.Database.EnsureCreated();

		// await SeedDatabaseAsync(dbContext, mediator);
	}

	private static async Task SeedDatabaseAsync(
		IApplicationDbContext dbContext,
		IMediator mediator)
	{
		if (dbContext.Sellers.Any() == false)
			await CreateSellerAsync(dbContext, mediator);

		dbContext.SaveChanges();
	}

	private static async Task CreateSellerAsync(
		IApplicationDbContext dbContext,
		IMediator mediator)
	{
        var seller = new Seller()
        {
            Id = Guid.NewGuid(),
            Name = "Bill Gates",
            Email = "gatesbill@gmail.com",
            Password = "CoolPassword 1225"
        };

        await dbContext.Sellers.AddAsync(seller);
        await dbContext.SaveChangesAsync();
	}
}
