using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.AssistingModels
{
    public class MemberUserPlanVM
    {
        public int MemberId { get; set; }
 
        public string StartDateTime { get; set; }

        public string ExpireDateTime { get; set; }
        public string CreatedDateTime { get; set; }
  
        public string Status { get; set; }
        //public AppUser AppUser { get; set; }
        //public SubPlan SubPlan { get; set; }

        // SubPlan Table
        public string PlanName { get; set; }

        // AppUser
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }

    }
}
