using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Users.Dtos
{
    public class AppUserDto
    {
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] ProfilePic { get; set; }
        public string Address { get; set; }
        public bool Deleted { get; set; }
    }
}
