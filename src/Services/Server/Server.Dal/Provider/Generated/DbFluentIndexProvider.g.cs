using Server.Dal.Model;
using Microsoft.EntityFrameworkCore;

namespace Server.Dal.Provider
{
    internal static class FluentIndex
    {
        public static void Config(ModelBuilder builder)
        {
            builder.Entity<UserRoleDb>()
				.HasIndex(x => new {x.UserId, x.RoleTypeId}, "ix_user_role_user_id_role_type_id_uindex")
				.IsUnique(true);

            builder.Entity<UserRoleTypeDb>()
				.HasIndex(x => new {x.Name}, "ix_user_role_type_name")
				.IsUnique(true);

		}
    }
}
