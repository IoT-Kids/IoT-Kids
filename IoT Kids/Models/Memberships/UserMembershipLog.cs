using IoT_Kids.Models.Payments;
using IoT_Kids.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Models.Memberships
{
    // to store all subscriptions of the user
    public class UserMembershipLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Status { get; set; }
        public int? PaymentId { get; set; }
        public int? MembershipPlanId { get; set; }
        public string Ref01 { get; set; }
        public string Ref02 { get; set; }
        public int Ref03 { get; set; }
        public int Ref04 { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("MembershipPlanId")]
        public MembershipPlan MembershipPlan { get; set; }

        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
    }
}
