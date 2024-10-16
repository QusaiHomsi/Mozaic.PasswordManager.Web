using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Web.Models.DBEntities;

namespace Mozaic.PasswordManager.DAL
{
    public class RepositoryBase
    {
        internal AppDbContext AppDbContext { get; set; }
        public RepositoryBase()
        {

            string connectionString = "server=desktop-o738fn0\\mssqlserver1;database=PasswordManager;user=sa;password=11111111;trustservercertificate=true";
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            AppDbContext = new AppDbContext(optionsBuilder.Options);
        }

    }
}
