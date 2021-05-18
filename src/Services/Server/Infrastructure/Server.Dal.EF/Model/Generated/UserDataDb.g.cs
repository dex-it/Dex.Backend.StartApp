using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Bll.Abstraction.Common;
using Dex.Ef.Contracts.Entities;

namespace Server.Dal.EF.Model
{
	[Table("user_data")]
	public partial class UserDataDb : IEntity<Guid>, IDbEntity
	{

		[Key]
		[Column("id")]
		public Guid Id { get; set; }

		[Column("user_id")]
		public Guid UserId { get; set; }

		[Column("full_name")]
		public string FullName { get; set; }

		[Column("birth_day")]
		public Nullable<DateTime> BirthDay { get; set; }

		[Column("gendor")]
		public Gendor Gendor { get; set; }

		[ForeignKey(nameof(UserId))]
		public UserDb User { get; set; }
	}
}