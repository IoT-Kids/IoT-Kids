using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string Address { get; set; }
        public byte[] ProfilePic { get; set; }
        public bool Deleted { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
    }
}
