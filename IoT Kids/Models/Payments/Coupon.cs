using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Payments
{
    public class Coupon
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } // Coupon code
        public string Description { get; set; }
        [Required]
        public double DiscountAmt { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Ref01 { get; set; }
        public string Ref02 { get; set; }
        public int Ref03 { get; set; }
        public int Ref04 { get; set; }
    }
}
