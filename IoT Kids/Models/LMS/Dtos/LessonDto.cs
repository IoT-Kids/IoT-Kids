using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS.Dtos
{
    public class LessonDto
    {
        public int Id { get; set; }
        public int CourseID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; } // in case the lesson does not contain a video
        public string VideoURL { get; set; }
        public bool SampleVideo { get; set; }
        public int Index { get; set; }
        public bool HasTest { get; set; }
        public string Status { get; set; }
    }
}
