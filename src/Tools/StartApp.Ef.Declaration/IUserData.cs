using System;
using Dex.DalGenerator.Annotation.Attributes;
using Dex.Ef.Contracts.Entities;

namespace StartApp.Ef.Declaration
{
    public interface IUserData : IEntity<Guid>
    {
        [References(typeof(IUser), OneToOne = true)]
        Guid UserId { get; }

        string FullName { get; }

        DateTime? BirthDay { get; }
    }
}