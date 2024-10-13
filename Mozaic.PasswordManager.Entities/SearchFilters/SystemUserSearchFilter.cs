using System;

namespace Mozaic.PasswordManager.Entities.SearchFilters
{
    public class SystemUserSearchFilter
    {
        public int? Id { get; set; } 
        public string UserName { get; set; }
        public bool? IsAdmin { get; set; }  
    }
}
