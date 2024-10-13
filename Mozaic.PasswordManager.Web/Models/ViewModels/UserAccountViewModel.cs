using System.ComponentModel.DataAnnotations;

namespace Mozaic.PasswordManager.Web.Models.ViewModels
{
    public class UserAccountViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string EncryptionKey { get; set; }
    }
}
