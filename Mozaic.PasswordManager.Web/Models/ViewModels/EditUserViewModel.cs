namespace Mozaic.PasswordManager.Web.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }  // User ID

        public string UserName { get; set; }  // Username of the user

        public bool IsAdmin { get; set; }  // Admin status
    }
}
