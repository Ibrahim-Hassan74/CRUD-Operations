using Microsoft.AspNetCore.Mvc;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already exist")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} should contains numbers only")]
        public string? Phone { get; set; }
        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
        [Required(ErrorMessage = "{0} can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
