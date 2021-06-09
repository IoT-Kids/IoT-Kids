using IoT_Kids.Data;
using IoT_Kids.Models;
using IoT_Kids.Repositories.IRepositories.IMembers;
using IoT_Kids.Repositories.IRepositories.IMembershipPlans;
using IoT_Kids.Repositories.IRepositories.IMembershipTrans;
using IoT_Kids.Repositories.IRepositories.IUsers;
using IoT_Kids.Repositories.IRepositories.Payments;
using IoT_Kids.StaticDetails;
using IoT_Kids.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.MembershipTrans
{
    public class MembershipTranRepo : IMembershipTranRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserRepo _userRepo;
        private readonly IMembershipPlanRepo _MemPlanRepo;
        private readonly IMemberRepo _memberRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly ICouponRepo _CouponRepo;
        private readonly IUserMembershipLogRepo _userMemLogRepo;
        public MembershipTranRepo(ApplicationDbContext db, IUserRepo userRepo, IMembershipPlanRepo memPlanRepo,
            IMemberRepo memberRepo, IPaymentRepo paymentRepo, 
            ICouponRepo CouponRepo, IUserMembershipLogRepo userMemLogRepo)
        {
            _db = db;
            _userRepo = userRepo;
            _MemPlanRepo = memPlanRepo;
            _memberRepo = memberRepo;
            _paymentRepo = paymentRepo;
            _CouponRepo = CouponRepo;
            _userMemLogRepo = userMemLogRepo;
        }
        
        public async Task<bool> CreateUserMembership(UserMembershipVM UserMember)
        {
            // Start Transaction to create new user, member and a membership
            var Transaction = _db.Database.BeginTransaction();
            try
            {
                // Create user record
                await _userRepo.RegisterUser(UserMember.FullName, UserMember.Email, UserMember.Phone, UserMember.UserAddress, UserMember.Password);

                // get the newly created user
                var NewUser = await _userRepo.GetUser(UserMember.Email);

                // Getting Membership plan
                var MemPlanObj = await _MemPlanRepo.GetPlanbyId(UserMember.MembershipPlanId);

                
                if (NewUser == null || MemPlanObj == null)
                {
                    Transaction.Rollback();
                    return false;
                }


                // Create member record
                Member NewMember = new Member()
                {
                    UserId = NewUser.Id,
                    MembershipPlanId = UserMember.MembershipPlanId,
                    StartDateTime = DateTime.Now,
                    CreatedDateTime = DateTime.Now,
                    Status = SD.ActiveMember
                };

                // If limited duration set for the selected plan, then we need to get the access period
                if (MemPlanObj.HasLimitedDuration)
                {
                    NewMember.ExpireDateTime = NewMember.StartDateTime.AddDays(MemPlanObj.Period);
                }

                // Create Payment record

                await _memberRepo.CreateMember(NewMember);

                Payment NewPayment = new Payment
                {
                    UserId = NewUser.Id,
                    MembershipPlanId = UserMember.MembershipPlanId,
                    OrderNo = UserMember.OrderNo,
                    UnitPrice = MemPlanObj.Price,
                    Currency = MemPlanObj.Currency,
                    CouponId = UserMember.CouponId,
                    PaymentMethod = UserMember.PaymentMethod,
                    PaymentStatus = SD.SuccessfulPayment,
                    CreatedDateTime = DateTime.Now,
                    DiscountAmt = 0
            };
                // Apply discount if there is coupon code/ coupon is active
 
                if(UserMember.CouponId != null)
                {
                    var CouponObj = await _CouponRepo.GetCouponActiveById(UserMember.CouponId ?? 0);
                    if(CouponObj != null)
                    {
                        
                        NewPayment.DiscountAmt = CouponObj.DiscountAmt;
                    }
                }
                NewPayment.TotalNetAmt = NewPayment.UnitPrice - NewPayment.DiscountAmt ?? 0;
                await _paymentRepo.CreatePayment(NewPayment);


                // Create Payment and membership Log
                UserMembershipLog NewMemLog = new UserMembershipLog
                {
                    UserId = NewUser.Id,
                    StartDate = DateTime.Now,
                    ExpireDate = NewMember.ExpireDateTime,
                    PaymentId = NewPayment.Id,
                    MembershipPlanId = UserMember.MembershipPlanId

                };
                await _userMemLogRepo.CreateMembershipLog(NewMemLog);

                Transaction.Commit();
                return true;
            }
            catch
            {
                Transaction.Rollback();
                return false;
            }
        }


        public async Task<bool> RenewUserMembership(UpdateUserMembershipVM UserMember)
        {
            // Start Transaction to create new member (if does not exsist) or 
            //update current member and a membership

            var Transaction =  _db.Database.BeginTransaction();
            try
            {

                // update user membership
                // but first if it exists 
                var MemberObj = await _memberRepo.GetMemberByUserId(UserMember.UserId);

                // Getting membership plan
                var MemPlanObj = await _MemPlanRepo.GetPlanbyId(UserMember.MembershipPlanId);

                if (MemberObj == null || MemPlanObj == null)
                {
                    return false;
                }

                MemberObj.Status = SD.ActiveMember;


                // Get user obj
                //var UserObj = await _userRepo.GetUserbyId(UserMember.UserId);





                // if the user has no membership(a user whos not a customer yet for whatever reason)
                //, now create a new membership for the user
                // otherwise just get the user's membership
                //if (MemberObj == null)
                //{
                //    // Create member record
                //    Member NewMember = new Member()
                //    {
                //        UserId = UserMember.UserId,
                //        MembershipPlanId = UserMember.MembershipPlanId,
                //        StartDateTime = DateTime.Now,
                //        CreatedDateTime = DateTime.Now,
                //        Status = SD.ActiveMember
                //    };

                //    // If limited duration set for the selected plan, then we need to get the access period
                //    if (MemPlanObj.HasLimitedDuration)
                //    {
                //        NewMember.ExpireDateTime = NewMember.StartDateTime.AddDays(MemPlanObj.Period);
                //    }
                //    // Create member record
                //    await _memberRepo.CreateMember(NewMember);
                //}
                //// if (most of the cases) member exists for the user, 
                ////then update status, planId and start and expire date
                //else
                //{
                //    // where is status??
                //    MemberObj.MembershipPlanId = UserMember.MembershipPlanId;
                //    MemberObj.StartDateTime = DateTime.Now;
                //    MemberObj.Status = SD.ActiveMember;
                //    if (MemPlanObj.HasLimitedDuration)
                //    {
                //        MemberObj.ExpireDateTime = MemberObj.StartDateTime.AddDays(MemPlanObj.Period);
                //    }
                //    await _memberRepo.UpdateMember(MemberObj);
                //}
               

                // Create new payment 
                Payment NewPayment = new Payment
                {
                    UserId = UserMember.UserId,
                    MembershipPlanId = UserMember.MembershipPlanId,
                    OrderNo = UserMember.OrderNo, // we need to decide this order no. is it generated by front end or what
                    UnitPrice = MemPlanObj.Price,
                    Currency = MemPlanObj.Currency,
                    CouponId = UserMember.CouponId,
                    PaymentMethod = UserMember.PaymentMethod,
                    PaymentStatus = SD.SuccessfulPayment,
                    CreatedDateTime = DateTime.Now,
                    DiscountAmt = 0
                };
                // Apply discount if there is coupon code/ coupon is active

                if (UserMember.CouponId != null)
                {
                    var CouponObj = await _CouponRepo.GetCouponActiveById(UserMember.CouponId ?? 0);
                    if (CouponObj != null)
                    {
                        NewPayment.DiscountAmt = CouponObj.DiscountAmt;
                    }
                }
                NewPayment.TotalNetAmt = NewPayment.UnitPrice - NewPayment.DiscountAmt ?? 0;
                await _paymentRepo.CreatePayment(NewPayment);


                // Create Payment and membership Log
                UserMembershipLog NewMemLog = new UserMembershipLog
                {
                    UserId = UserMember.UserId,
                    StartDate = DateTime.Now,
                    ExpireDate = MemberObj.ExpireDateTime,
                    PaymentId = NewPayment.Id,
                    MembershipPlanId = UserMember.MembershipPlanId
                };
                await _userMemLogRepo.CreateMembershipLog(NewMemLog);

                Transaction.Commit();
                return true;
            }
            catch
            {
                Transaction.Rollback();
                return false;
            }
        }
    }
}
