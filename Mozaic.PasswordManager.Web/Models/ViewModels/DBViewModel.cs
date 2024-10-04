using System.ComponentModel.DataAnnotations;

namespace Mozaic.PasswordManager.Web.Models.ViewModels
{

    public class SystemUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
   
   



    public class UserAccountViewModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Account Name is required")]
            [StringLength(100, ErrorMessage = "Account Name cannot exceed 100 characters")]
            public string AccountName { get; set; }

            [Required(ErrorMessage = "Username is required")]
            [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }

            
            public int SystemUserId { get; set; }
            public SystemUserViewModel SystemUser { get; set; }
        }
    }

