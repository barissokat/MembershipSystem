using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MembershipSystem.Models
{
    public class CreateNewPassword : User
    {
        [Required, Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required, Display(Name = "New Password Again")]
        public string NewPassword2 { get; set; }
    }
}