using ApiTools.Domain.Options;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiTools.Data.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseDbOptions(this DbContextOptionsBuilder builder, string connectionString)
            => builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
