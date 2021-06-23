using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.LMS
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; } // optional 
        [Required]
        public string Question { get; set; }
        public int Index { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Ref01 { get; set; }
        public int Ref02 { get; set; }
        public string Ref03 { get; set; }
        public string Refo4 { get; set; }

        [ForeignKey("TestId")]
        public Test Test { get; set; }
 

    }
}
