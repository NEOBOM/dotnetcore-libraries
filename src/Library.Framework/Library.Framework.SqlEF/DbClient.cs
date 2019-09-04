using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Library.Framework.SqlEF
{
    public class DbClient : DbContext
    {
        public readonly IConfiguration _configuration = null;

        public DbClient()
        {
            _configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
        }

        public DbClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

    }
}
