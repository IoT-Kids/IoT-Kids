using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS.Dtos
{
    public class CrUpCourseDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public string Lang { get; set; }
        public string Status { get; set; }
        public int Order { get; set; }
    }
}
