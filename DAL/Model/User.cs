using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class User : DbEntity
    {
        [Required(ErrorMessage = "Username is required!")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "PwdHash is required!")]
        public string PwdHash { get; set; } = null!;

        [Required(ErrorMessage = "PwdSalt is required!")]
        public string PwdSalt { get; set; } = null!;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
