using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models
{
    public class Member
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int MembershipPlanId { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }

        public DateTime ExpireDateTime { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Ref01 { get; set; }
        public string Ref02 { get; set; }
        public int Ref03 { get; set; }
        public int Ref04 { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("MembershipPlanId")]
        public MembershipPlan MembershipPlan { get; set; }
    }
}
