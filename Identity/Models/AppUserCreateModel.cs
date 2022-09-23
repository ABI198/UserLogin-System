using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class AppUserCreateModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage ="You must enter a suitable email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and Password Confirmation missmatching")]
        [Required(ErrorMessage = "Password Confirmation is required")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
    }
}
