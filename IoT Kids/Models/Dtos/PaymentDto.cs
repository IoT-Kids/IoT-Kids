using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Dtos
{
    public class PaymentDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }
 
        public int OrderNo { get; set; } // auto generated number

        public int MembershipPlanId { get; set; }
        public string UserAddress { get; set; } // either Iraq or other countries
  
        public double UnitPrice { get; set; }
   
        public string Currency { get; set; }
        public int CouponId { get; set; }
        public double? DiscountAmt { get; set; }

        public double TotalNetAmt { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
