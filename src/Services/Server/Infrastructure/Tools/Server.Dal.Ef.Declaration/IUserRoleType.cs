using System;
using Dex.Ef.Attributes;
using Dex.Ef.Contracts.Entities;

namespace Server.Dal.Ef.Declaration
{
    public interface IUserRoleType : IEntity<Guid>
    {
        [Index(IsUnique = true)]
        string Name { get; }
    }
}