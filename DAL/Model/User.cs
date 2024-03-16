using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class User : DbEntity
    {
        [Required(ErrorMessage = "Username is required!")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "PwdHash is required!")]
        public string PwdHash { get; set; } = null!;

        [Required(ErrorMessage = "PwdSalt is required!")]
        public string PwdSalt { get; set; } = null!;

        [Required(ErrorMessage = "SecurityToken is required!")]
        public string SecurityToken { get; set; } = null!;
    }
}
