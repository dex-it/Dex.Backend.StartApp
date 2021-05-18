using Server.Bll.Abstraction.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace Server.Dal.EF.Provider
{
    internal static class EnumFluentDbProvider
    {
	    public static void MapEnum()
		{
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Gendor>(nameof(Gendor).ToLower());
		}

	    private static string[] GetNames(Type enym)
	    {
		    var regex = new Regex(@"([A-Z])");
		    var result = Enum.GetNames(enym)
			    .Select(e => regex.Replace(e, match => $"_{match.Value.ToLower()}").Remove(0, 1));
		    return result.ToArray();
	    }

        public static void Config(ModelBuilder builder)
        {
            builder.HasPostgresEnum(nameof(Gendor).ToLower(), GetNames(typeof(Gendor)));

		}
    }
}