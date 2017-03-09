using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MembershipSystem.Models
{
    public class User
    {
        public string Id { get; set; }

        [Required, Display(Name = "User Name")]
        public string Name { get; set; }
    }
}