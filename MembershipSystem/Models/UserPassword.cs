using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MembershipSystem.Models
{
    public class UserPassword : User
    {
        [Required]
        public string Password { get; set; }

        [Required, Display(Name = "Password Again")]
        public string Password2 { get; set; }
    }
}