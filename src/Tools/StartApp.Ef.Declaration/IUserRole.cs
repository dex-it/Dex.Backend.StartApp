using System;
using Dex.DalGenerator.Annotation.Attributes;
using Dex.Ef.Attributes;
using Dex.Ef.Contracts.Entities;

namespace StartApp.Ef.Declaration
{
    public interface IUserRole : IEntity<Guid>
    {
        [Index("ix_user_role_user_id_role_type_id_uindex", IsUnique = true)]
        [References(typeof(IUser))]
        Guid UserId { get; }

        [Index("ix_user_role_user_id_role_type_id_uindex", 1, IsUnique = true)]
        [References(typeof(IUserRoleType))]
        Guid RoleTypeId { get; }
    }
}