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
   
   



    
    }

