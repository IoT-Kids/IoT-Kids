using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS.Dtos
{
    public class QuestionChoiceDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Index { get; set; }
        public string Choice { get; set; }
        public bool Correct { get; set; } // if it is the correct choice
       // public byte[] Image { get; set; }
    }
}
