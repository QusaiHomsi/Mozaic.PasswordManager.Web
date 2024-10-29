namespace Mozaic.PasswordManager.Entities.SearchFilters
{
    public class UserAccountSearchFilter : SearchFilterBase
    {
        public string UserName { get; set; }
        public int PageNumber { get; set; } = 1; 
        public int PageSize { get; set; } = 5; 
    }
}
