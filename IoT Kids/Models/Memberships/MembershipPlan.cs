using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Memberships
{
    public class MembershipPlan
    {
        public int Id { get; set; }
        [Required]
        public string PlanName { get; set; }
        public bool HasLimitedDuration { get; set; }
        public int Period { get; set; } // in days
        [Required]
        public string PlanStatus { get; set; }
        [Required]
        public double Price { get; set; }
        public string Currency { get; set; }
        public double ForeignPrice { get; set; }
        public string ForeignCurrency { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Ref01 { get; set; }
        public string Ref02 { get; set; }
        public int Ref03 { get; set; }
        public int Ref04 { get; set; }

    }
}
