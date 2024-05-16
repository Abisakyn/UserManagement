using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserManagement.Enums;

namespace UserManagement.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name cant be empty")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email cant be empty")]
        [EmailAddress(ErrorMessage ="Email should be in proper format")]
        [Remote(action:"IsEmailAlreadyRegister",controller:"Account",ErrorMessage ="Email is already in use")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number cant be empty")]
        public string? PhoneNumber { get; set; }
       
        [Required(ErrorMessage = "IdNumber cant be empty")]
        public string? IdNumber { get; set; }

        [Required(ErrorMessage = "Password cant be empty")]
        public string? Password { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
