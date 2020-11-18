using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // A hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole>()
                {
                    new IdentityRole
                    {
                        Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR".ToUpper()
                    },
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var users = new List<IdentityUser>()
                {
                    new IdentityUser
                    {
                        Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                        UserName = "admin@gmail.com",
                        NormalizedUserName = "admin@gmail.com".ToUpper(),
                        Email = "admin@gmail.com",
                        NormalizedEmail = "admin@gmail.com".ToUpper(),
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, "Pass123$")
                    },
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.UserRoles.Any())
            {
                var userRoles = new List<IdentityUserRole<string>>
                {
                    new IdentityUserRole<string>
                    {
                        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                    },
                };
                context.UserRoles.AddRange(userRoles);
                context.SaveChanges();
            }
        }

        public static void RecreateDatabase(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}
