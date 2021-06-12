using IoT_Kids.Models.Memberships;
using IoT_Kids.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Payments
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int OrderNo { get; set; } // auto generated number
        [Required]
        public int MembershipPlanId { get; set; }
        public string UserAddress { get; set; } // either Iraq or other countries
        [Required]
        public double UnitPrice { get; set; } 
        [Required]
        public string Currency { get; set; }
        public int? CouponId { get; set; }
        public double? DiscountAmt { get; set; }
        [Required]
        public double TotalNetAmt { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Ref01 { get; set; }
        public string Ref02 { get; set; }
        public int? Ref03 { get; set; }
        public int? Ref04 { get; set; }
        public double? Ref05 { get; set; }
        public double? Ref06 { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("MembershipPlanId")]
        public MembershipPlan MembershipPlan { get; set; }

        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

    }
}
