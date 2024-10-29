using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozaic.PasswordManager.Entities
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }


        [ForeignKey("SystemUser")]
        public int UserId { get; set; }

        public SystemUser SystemUser { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
