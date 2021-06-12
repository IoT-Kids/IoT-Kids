using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Memberships.Dtos
{
    public class MemberUpdateDto
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
    }
}
