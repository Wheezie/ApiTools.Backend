using ApiTools.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApiTools.Data
{
    public class ApiDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
    {
        public ApiDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .Build();

            DbContextOptionsBuilder<ApiDbContext> optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
            optionsBuilder.UseDbOptions(config.GetConnectionString("ApiTools"));

            return new ApiDbContext(optionsBuilder.Options);
        }
    }
}
