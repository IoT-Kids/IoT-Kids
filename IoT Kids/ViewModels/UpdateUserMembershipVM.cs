using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.AssistingModels
{
    //this class should update user  membership status
    // will update Member table (update the status and planId and start and expiry dates)
    // will also create record in the following tables:
    //Payment, UserMembershipLog
    public class UpdateUserMembershipVM
    {
        // User table
        [Required]
        public string UserId { get; set; }

        // Update Member table
        // prop PlanId
        // membership status 

        // MembershipPlan table (Membership plan)
        [Required]
        public int MembershipPlanId { get; set; }

        // UserMembershipLog table (user Membershipscription log)

        //payment table
        public int? CouponId { get; set; }
        public int OrderNo { get; set; }
        public string PaymentMethod { get; set; } // got to be static value


    }
}
