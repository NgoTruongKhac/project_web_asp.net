using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models.Account
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(255)]
        public string Pass { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [StringLength(10)]
        public string? Sex { get; set; } // Male/Female/Other

        public DateTime? Birthday { get; set; }

        [StringLength(255)]
        public string? Avatar { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "customer";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User(string? firstName, string? lastName, string pass, string? email)
        {

            FirstName = firstName;
            LastName = lastName;
            Pass = pass;
            Email = email;


        }
        public List<Address> addresses { get; set; } = new List<Address>();
    }
}
