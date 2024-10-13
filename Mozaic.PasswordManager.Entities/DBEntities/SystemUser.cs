using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.Entities
{
    public class SystemUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string password { get; set; }


        public bool? IsAdmin { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? LastModified { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }
    }
}
