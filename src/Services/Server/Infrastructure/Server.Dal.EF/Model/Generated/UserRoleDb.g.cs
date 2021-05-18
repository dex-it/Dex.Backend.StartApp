using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Bll.Abstraction.Common;
using Dex.Ef.Contracts.Entities;

namespace Server.Dal.EF.Model
{
	[Table("user_role")]
	public partial class UserRoleDb : IEntity<Guid>, IDbEntity
	{

		[Key]
		[Column("id")]
		public Guid Id { get; set; }

		[Column("user_id")]
		public Guid UserId { get; set; }

		[Column("role_type_id")]
		public Guid RoleTypeId { get; set; }

		[ForeignKey(nameof(UserId))]
		public UserDb User { get; set; }
		[ForeignKey(nameof(RoleTypeId))]
		public UserRoleTypeDb RoleType { get; set; }
	}
}