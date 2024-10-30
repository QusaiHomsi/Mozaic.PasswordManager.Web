using Microsoft.EntityFrameworkCore;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Mozaic.PasswordManager; 


namespace Mozaic.PasswordManager.DAL
{
    public class RepositoryBase
    {
        internal AppDbContext AppDbContext { get; set; }

        public RepositoryBase()
        {            
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(DatabaseHelper.ConnectionString);
            AppDbContext = new AppDbContext(optionsBuilder.Options);
        }
    }
}
