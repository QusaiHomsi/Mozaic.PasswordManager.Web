using System;

namespace Mozaic.PasswordManager.Entities.SearchFilters
{
    public class SystemUserSearchFilter
    {
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public bool? IsAdmin { get; set; }
        public int PageSize { get; set; } = 5; 
        public int PageNumber { get; set; } = 1;
        public int TotalRecords { get; set; }
    }

}
