using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myCircle.Models
{
    public class Login
    {
        [Required(ErrorMessage="Email is required")]
        [Display(Name = "Email:")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string email{get;set;}
        
        [Required(ErrorMessage="Password is required")]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        public string password{get;set;}
        
    }
}