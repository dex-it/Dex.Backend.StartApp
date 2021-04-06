using Server.Dal.Model;
using Microsoft.EntityFrameworkCore;

namespace Server.Dal.Provider
{
    internal static class ForeignKeysFluentDbProvider
    {
        public static void Config(ModelBuilder builder)
        {
			builder.Entity <UserDb>()
				.HasOne(d => d.UserData)
				.WithOne(u => u.User)
				.HasForeignKey<UserDataDb>(d => d.UserId)
				.OnDelete(DeleteBehavior.Restrict);
                        
			builder.Entity <UserRoleDb>()
				.HasOne(d => d.User)
				.WithMany(u => u.UserRole)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity <UserRoleDb>()
				.HasOne(d => d.RoleType)
				.WithMany(u => u.UserRole)
				.HasForeignKey(d => d.RoleTypeId)
				.OnDelete(DeleteBehavior.Restrict);

		}
    }
}