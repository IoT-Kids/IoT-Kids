using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.AssistingModels
{
    // this class contains props needed to create a membership for first time user
    // the props to create record in the following tables:
    //AppUser, Member, Payment and UserSubLog
    public class UserMembershipVM
    {
        // User table
        // [Required]
        public string UserId { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
    
        public string Password { get; set; }
        public string UserAddress { get; set; } // we gonna make it Iraq for now

        // Member table
        // prop UserId
        // prop PlanId

        // SubPlan table (Subscription plan)
        [Required]
        public int MembershipPlanId { get; set; }



        // UserSubLog table (user subscription log)

        //payment table
        public int? CouponId { get; set; }
        public int OrderNo { get; set; }
        public string PaymentMethod { get; set; } // got to be static value


    }
}
