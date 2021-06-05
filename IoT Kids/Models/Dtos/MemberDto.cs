using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
       // [Required]
        public int MembershipPlanId { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }

        public DateTime ExpireDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        [Required]
        public string Status { get; set; }
        public AppUser AppUser { get; set; }
        public MembershipPlan MembershipPlan { get; set; }

 
    }
}
