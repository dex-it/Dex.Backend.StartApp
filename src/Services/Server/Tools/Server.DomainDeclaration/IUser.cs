using System;
using System.ComponentModel.DataAnnotations;
using Dex.Ef.Contracts.Entities;

namespace Server.DomainDeclaration
{
    public interface IUser : IEntity<Guid>
    {
        string Login { get; }

        string Email { get; }

        string Password { get; }

        string Salt { get; }

        [Required]
        Guid SecurityTimestamp { get; }
    }
}