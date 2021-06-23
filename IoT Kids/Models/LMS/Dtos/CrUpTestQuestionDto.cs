using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS.Dtos
{
    public class CrUpTestQuestionDto
    {
        public int TestId { get; set; }
        public string Title { get; set; } // optional 
        [Required]
        public string Question { get; set; }
       // public int Index { get; set; }
    }
}
