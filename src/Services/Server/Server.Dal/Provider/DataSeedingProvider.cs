using System;
using Microsoft.EntityFrameworkCore;
using Server.Dal.Common;
using Server.Dal.Model;

namespace Server.Dal.Provider
{
    internal class DataSeedingProvider
    {
        public static void Config(ModelBuilder builder)
        {
            RoleTypeSeeding(builder);
            AdminSeeding(builder);
        }
        
        private static void RoleTypeSeeding(ModelBuilder builder)
        {
            var roleTypes = new[]
            {
                new UserRoleTypeDb {Id = Guid.Parse("5bd7d6bc-6cac-4c61-8a42-0a90fd400eab"), Name = UserRoleNames.User},
                new UserRoleTypeDb {Id = Guid.Parse("ffa2836e-594b-4db2-95b7-098932f31361"), Name = UserRoleNames.Admin}
            };
        
            builder.Entity<UserRoleTypeDb>().HasData(roleTypes);
        }
        
        private static void AdminSeeding(ModelBuilder builder)
        {
            var id = Guid.Parse("D8C49692-E2C7-4125-9476-C7D5E4A325B7");
            var salt = Guid.Parse("8A57AD97-45FE-4DB1-89C4-3973E0852177").ToString();
            var userDb = new UserDb
            {
                Id = id,
                Salt = salt,
                Email = "admin@mail.com",
                Login = "admin@mail.com",
                SecurityTimestamp = Guid.Parse("4569B77D-8296-4F2F-AE9E-68F382F88B08"),
                Password = "10-57-FD-75-B5-7A-95-53-F0-B7-92-16-4C-01-5F-8C" //admin
            };
        
            var userRoles = new UserRoleDb
            {
                Id = Guid.Parse("E13354BA-96D7-472F-92AB-D870E2B016C0"),
                RoleTypeId = Guid.Parse("ffa2836e-594b-4db2-95b7-098932f31361"),
                UserId = id
            };
        
            var userData = new UserDataDb
            {
                Id = Guid.Parse("857D6A4E-397E-4FFD-B146-294A1B65FC8D"),
                UserId = id,
                BirthDay = new DateTime(1990, 4, 12, 0, 0, 0, DateTimeKind.Utc),
                FullName = "admin",
            };
        
            builder.Entity<UserDb>().HasData(userDb);
            builder.Entity<UserRoleDb>().HasData(userRoles);
            builder.Entity<UserDataDb>().HasData(userData);
        }
    }
}