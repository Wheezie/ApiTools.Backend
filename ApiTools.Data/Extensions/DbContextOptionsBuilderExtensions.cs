using ApiTools.Domain.Options;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiTools.Data.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseDbOptions(this DbContextOptionsBuilder builder, string connectionString)
        {
            ServerVersion serverVersion;
            if (!ServerVersion.TryFromString(connectionString, out serverVersion))
            {
                throw new ArgumentException("Connectionstring is invalid, or the server cannot be determined.");
            }

            return builder.UseMySql(serverVersion);
        }
    }
}
