using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace iStart.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(75, MinimumLength = 3), RegularExpression(@"^[a-zA-Z\s]*$")]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string EncryptedPassword { get; set; }

        [NotMapped]
        [Required, StringLength(12, MinimumLength = 8), RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,12}$")]
        public string Password { get; set; }

        public string Status { get; set; } = "Active";
    }
}
