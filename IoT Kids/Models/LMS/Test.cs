using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS
{
    public class Test
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        [Required]
        public int Type { get; set; } // mid of final test. 1 = mid, 2 = final
        public int? LessonId { get; set; } // only if it's mid test
        [Required]
        public int PassingPercent { get; set; } // passing percentage
        [Required]
        public string Status { get; set; }
        public int Index { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Ref01 { get; set; }
        public int Ref02 { get; set; }
        public string Ref03 { get; set; }
        public string Refo4 { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; }
    }
}
