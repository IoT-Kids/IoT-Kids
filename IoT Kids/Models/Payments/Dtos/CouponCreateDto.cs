using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Payments.Dtos
{
    public class CouponCreateDto
    {
  
        [Required]
        public string Code { get; set; } // Coupon code
        public string Description { get; set; }
        [Required]
        public double DiscountAmt { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
