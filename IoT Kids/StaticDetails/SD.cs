using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.StaticDetails
{
    public static class SD
    {
        //Membership Status
        public const string ActiveMember = "Active";
        public const string ExpiredMember = "Expired";

        //Coupon Status
        public const string ActiveCoupon = "Active";
        public const string InactiveCoupon = "Inactive";

        //Payment Status
        public const string SuccessfulPayment = "Successful";
        public const string FailedPayment = "Failed";
        public const string CancelledPayment = "Cancelled";

        // Payment Method
        public const string ZainPaymentMethod = "Zain";
        public const string SwitchPaymentMethod = "Switch";
    }
}
