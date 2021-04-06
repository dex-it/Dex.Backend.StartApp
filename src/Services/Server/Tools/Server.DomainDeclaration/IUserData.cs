using System;
using Dex.DalGenerator.Annotation.Attributes;
using Dex.Ef.Contracts.Entities;
using Server.Dal.Common;

namespace Server.DomainDeclaration
{
    public interface IUserData : IEntity<Guid>
    {
        [References(typeof(IUser), OneToOne = true)]
        Guid UserId { get; }

        string FullName { get; }

        DateTime? BirthDay { get; }

        Gendor Gendor { get; }
    }
}