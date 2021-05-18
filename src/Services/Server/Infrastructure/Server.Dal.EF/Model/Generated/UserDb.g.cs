using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Bll.Abstraction.Common;
using Dex.Ef.Contracts.Entities;

namespace Server.Dal.EF.Model
{
	[Table("user")]
	public partial class UserDb : IEntity<Guid>, IDbEntity
	{

		[Key]
		[Column("id")]
		public Guid Id { get; set; }

		[Column("login")]
		public string Login { get; set; }

		[Column("email")]
		public string Email { get; set; }

		[Column("password")]
		public string Password { get; set; }

		[Column("salt")]
		public string Salt { get; set; }

		[Required]
		[Column("security_timestamp")]
		public Guid SecurityTimestamp { get; set; }

		public UserDataDb UserData { get; set; }
		public  System.Collections.Generic.ICollection<UserRoleDb> UserRole { get; set; }
	}
}