using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Web.Models.DBEntities;

namespace Mozaic.PasswordManager.DAL
{
    public class RepositoryBase
    {
        internal AppDbContext AppDbContext { get; set; }
        public RepositoryBase()
        {

            string connectionString = "ReplaceWithConnectionString";
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            AppDbContext = new AppDbContext(optionsBuilder.Options);
        }

    }
}
