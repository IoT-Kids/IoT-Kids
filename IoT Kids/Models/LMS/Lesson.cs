using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS
{
    public class Lesson
    {
        public int Id { get; set; }
        public int CourseID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; } // in case the lesson does not contain a video
        public string VideoURL { get; set; }
        public bool SampleVideo { get; set; }
        [Required]
        public int Index { get; set; }
        public bool HasTest { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Ref01 { get; set; }
        public int Ref02 { get; set; }
        public string Ref03 { get; set; }
        public string Refo4 { get; set; }

        [ForeignKey("CourseID")]
        public Course Course { get; set; }

    }
}
