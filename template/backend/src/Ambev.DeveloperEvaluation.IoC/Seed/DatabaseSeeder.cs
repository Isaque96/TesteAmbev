using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Seed;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.IoC.Seed;

public class DatabaseSeeder(DefaultContext context, IPasswordHasher passwordHasher) : IDatabaseSeeder
{
    public async Task SeedAsync()
    {
        await context.Database.MigrateAsync();

        await SeedUsersAsync();
        await SeedCategoriesAndProductsAsync();
        await SeedCartsAsync();

        await context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        if (await context.Users.AnyAsync())
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Email = "admin@ambev.dev",
            Phone = "(11) 99999-0000",
            Password = passwordHasher.HashPassword("Admin@123"),
            Role = UserRole.Admin,
            Status = UserStatus.Active,
            Name = new Name
            {
                FirstName = "Admin",
                LastName = "User"
            },
            Address = new Address
            {
                City = "São Paulo",
                Street = "Av. Paulista",
                Number = 1000,
                ZipCode = "01310-000",
                Geolocation = new Geolocation
                {
                    Lat = "-23.561414",
                    Long = "-46.655881"
                }
            }
        };

        var customer = new User
        {
            Id = Guid.NewGuid(),
            Username = "cliente1",
            Email = "cliente1@ambev.dev",
            Phone = "(11) 98888-0000",
            Password = passwordHasher.HashPassword("Cliente@123"), // usa o hasher
            Role = UserRole.Customer,
            Status = UserStatus.Active,
            Name = new Name
            {
                FirstName = "João",
                LastName = "Silva"
            },
            Address = new Address
            {
                City = "São Paulo",
                Street = "Rua das Flores",
                Number = 123,
                ZipCode = "01000-000",
                Geolocation = new Geolocation
                {
                    Lat = "-23.550520",
                    Long = "-46.633308"
                }
            }
        };

        await context.Users.AddRangeAsync(admin, customer);
    }

    private async Task SeedCategoriesAndProductsAsync()
    {
        if (await context.Categories.AnyAsync() || await context.Products.AnyAsync())
            return;

        var cervejaCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Cervejas",
            Description = "Diversos tipos de cervejas"
        };

        var refrigeranteCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Refrigerantes",
            Description = "Bebidas não alcoólicas"
        };

        var produto1 = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Cerveja Lager 350ml",
            Description = "Cerveja tipo Lager, lata 350ml",
            Price = 4.99m,
            Image = "https://example.com/images/cerveja-lager-350ml.jpg",
            CategoryId = cervejaCategory.Id,
            Rating = new Rating
            {
                Rate = 4.5m,
                Count = 120
            }
        };

        var produto2 = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Cerveja IPA 600ml",
            Description = "Cerveja tipo IPA, garrafa 600ml",
            Price = 12.90m,
            Image = "https://example.com/images/cerveja-ipa-600ml.jpg",
            CategoryId = cervejaCategory.Id,
            Rating = new Rating
            {
                Rate = 4.8m,
                Count = 85
            }
        };

        var produto3 = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Refrigerante Cola 2L",
            Description = "Refrigerante sabor cola, garrafa 2L",
            Price = 8.50m,
            Image = "https://example.com/images/refrigerante-cola-2l.jpg",
            CategoryId = refrigeranteCategory.Id,
            Rating = new Rating
            {
                Rate = 4.2m,
                Count = 200
            }
        };

        await context.Categories.AddRangeAsync(cervejaCategory, refrigeranteCategory);
        await context.Products.AddRangeAsync(produto1, produto2, produto3);
    }

    private async Task SeedCartsAsync()
    {
        if (await context.Carts.AnyAsync())
            return;

        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "cliente1");
        if (user == null)
            return;

        var products = await context.Products.Take(2).ToListAsync();
        if (!products.Any())
            return;

        var cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Date = DateTime.UtcNow
        };

        var item1 = new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = cart.Id,
            ProductId = products[0].Id,
            Quantity = 3
        };

        var item2 = new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = cart.Id,
            ProductId = products[1].Id,
            Quantity = 6
        };

        cart.CartItems.Add(item1);
        cart.CartItems.Add(item2);

        await context.Carts.AddAsync(cart);
        await context.CartItems.AddRangeAsync(item1, item2);
    }
}