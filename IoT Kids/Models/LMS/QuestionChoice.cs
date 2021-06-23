using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS
{
    public class QuestionChoice
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Index { get; set; }
        [Required]
        public string Choice { get; set; }
        public bool Correct { get; set; } // if it is the correct choice
        public byte[] Image { get; set; }
        public int Ref01 { get; set; }
        public int Ref02 { get; set; }
        public string Ref03 { get; set; }
        public string Refo4 { get; set; }

        [ForeignKey("QuestionId")]
        public TestQuestion TestQuestion { get; set; }
    }
}
