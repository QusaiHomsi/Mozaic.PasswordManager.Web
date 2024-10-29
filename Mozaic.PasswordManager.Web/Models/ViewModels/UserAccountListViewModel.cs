using System.Collections.Generic;

namespace Mozaic.PasswordManager.Web.Models.ViewModels
{
    public class UserAccountListViewModel
    {
        public List<UserAccountViewModel> UserAccounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
