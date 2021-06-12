using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public string Lang { get; set; }
        public string Status { get; set; }

        public int Order { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int Ref01 { get; set; }
        public int Ref02 { get; set; }
        public string Ref03 { get; set; }
        public string Refo4 { get; set; }

    }
}
