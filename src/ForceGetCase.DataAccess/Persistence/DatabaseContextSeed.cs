using ForceGetCase.Core.Entities;
using ForceGetCase.DataAccess.Identity;
using Microsoft.AspNetCore.Identity;

namespace ForceGetCase.DataAccess.Persistence;

public static class DatabaseContextSeed
{
    public static async Task SeedDatabaseAsync(DatabaseContext context, UserManager<ApplicationUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new ApplicationUser { UserName = "admin", Email = "admin@admin.com", EmailConfirmed = true };
            
            await userManager.CreateAsync(user, "Admin123.?");
        }
        
        if (!context.Dimensions.Any())
        {
            foreach (var dimension in Dimension.ValidDimensions)
            {
                context.Dimensions.Add(dimension);
            }
        }
        
        if (!context.Countries.Any())
        {
            List<Country> countries =
            [
                new Country
                {
                    Name = "USA",
                    Cities =
                    [
                        new City { Name = "New York" },
                        new City { Name = "Los Angeles" },
                        new City { Name = "Miami" },
                        new City { Name = "Minnesota" },
                    ]
                },
                new Country
                {
                    Name = "China",
                    Cities =
                    [
                        new City { Name = "Beijing" },
                        new City { Name = "Shanghai" },
                    ]
                },
                new Country
                {
                    Name = "Turkey",
                    Cities =
                    [
                        new City { Name = "Istanbul" },
                        new City { Name = "Izmir" },
                    ]
                },
            ];
            
            foreach (var country in countries)
            {
                context.Countries.Add(country);
            }
        }
        
        await context.SaveChangesAsync();
    }
}
