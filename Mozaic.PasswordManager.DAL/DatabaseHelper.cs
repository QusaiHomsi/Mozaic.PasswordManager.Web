using Microsoft.Extensions.Configuration;

namespace Mozaic.PasswordManager.DAL
{
    internal static class DatabaseHelper
    {
        public static readonly string ConnectionString = null;

        static DatabaseHelper()
        {
            var _configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }
    }
    }

