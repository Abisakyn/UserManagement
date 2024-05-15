using System.ComponentModel.DataAnnotations;

namespace UserManagement.DTO
{
    public class LoginDTO

    {
        [Required(ErrorMessage ="Email cant be blank")]
        [EmailAddress(ErrorMessage ="Email address should be in proper format")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="Password cant be blank")]
        public string? Password { get; set; }
    }
}
