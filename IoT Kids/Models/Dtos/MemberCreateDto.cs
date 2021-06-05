using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Dtos
{
    public class MemberCreateDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int MembershipPlanId { get; set; }
    }
}
