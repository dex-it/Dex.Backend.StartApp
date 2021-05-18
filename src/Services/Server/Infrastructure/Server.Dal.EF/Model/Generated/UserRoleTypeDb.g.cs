using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Bll.Abstraction.Common;
using Dex.Ef.Contracts.Entities;

namespace Server.Dal.EF.Model
{
	[Table("user_role_type")]
	public partial class UserRoleTypeDb : IEntity<Guid>, IDbEntity
	{

		[Key]
		[Column("id")]
		public Guid Id { get; set; }

		[Column("name")]
		public string Name { get; set; }

		public  System.Collections.Generic.ICollection<UserRoleDb> UserRole { get; set; }
	}
}