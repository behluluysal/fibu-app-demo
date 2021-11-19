using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(12, MinimumLength = 3, ErrorMessage ="Firstname should be between 3-12 characters")]
        override public string UserName { get; set; }

        [Required]
        override public string Email { get; set; }
    }
}