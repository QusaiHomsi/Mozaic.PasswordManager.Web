using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mozaic.PasswordManager.Web.Models.DBEntities
{
    public class SystemUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string HashedPassword { get; set; }

      
        public bool? IsAdmin { get; set; } 

        public DateTime? CreationDate { get; set; }

        public DateTime? LastModified { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }
    }

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
    }
}
