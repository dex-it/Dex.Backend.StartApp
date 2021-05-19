using System;
using Dex.DalGenerator.Annotation.Attributes;
using Dex.Ef.Contracts.Entities;
using Server.Bll.Abstraction.Common;

namespace Server.Dal.Ef.Declaration
{
    public interface IUserData : IEntity<Guid>
    {
        [References(typeof(IUser), OneToOne = true)]
        Guid UserId { get; }

        string FullName { get; }

        DateTime? BirthDay { get; }

        Gender Gender { get; }
    }
}