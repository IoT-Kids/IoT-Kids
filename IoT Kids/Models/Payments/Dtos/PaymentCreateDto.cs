using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Payments.Dtos
{
    public class PaymentCreateDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int MembershipPlanId { get; set; }
        public int OrderNo { get; set; }
        public int? CouponId { get; set; }
        public string PaymentMethod { get; set; }
 
    }
}
