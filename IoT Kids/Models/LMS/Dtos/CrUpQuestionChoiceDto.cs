using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS.Dtos
{
    public class CrUpQuestionChoiceDto
    {
        public int QuestionId { get; set; }
        [Required]
        public string Choice { get; set; }
        public bool Correct { get; set; } // if it is the correct choice
        //public byte[] Image { get; set; }
    }
}
