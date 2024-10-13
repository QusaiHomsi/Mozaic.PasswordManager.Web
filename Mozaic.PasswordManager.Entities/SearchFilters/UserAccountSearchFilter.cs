using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.Entities.SearchFilters
{
    public class UserAccountSearchFilter:SearchFilterBase
    {
        public string UserName { get; set; }
    }
}
