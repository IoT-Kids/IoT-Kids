using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS.Dtos
{
    public class CrUpTestDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
     
        public int Type { get; set; } // mid of final test
        public int? LessonId { get; set; } // only if it's mid test
   
        public int PassingPercent { get; set; }
  
        public string Status { get; set; }
    }
}
