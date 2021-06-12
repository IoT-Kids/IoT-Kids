using IoT_Kids.Data;
using IoT_Kids.Models;
using IoT_Kids.Models.Payments;
using IoT_Kids.Repositories.IRepositories.IPayments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.Payments
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly ApplicationDbContext _db;

        public PaymentRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreatePayment(Payment Payment)
        {
            _db.Payment.Add(Payment);
            return await Save();
        }

        public async Task<bool> DeletePayment(Payment PaymentObj)
        {
            // we have then to remove the payment from User membership Log first 
            var UserMembershipLogObj = _db.UserMembershipLog.FirstOrDefault(u => u.PaymentId == PaymentObj.Id);

            if(UserMembershipLogObj != null)
            {
                _db.UserMembershipLog.Remove(UserMembershipLogObj);
                await Save();
            }
            
            _db.Payment.Remove(PaymentObj);
            return await Save();
        }

        public async Task<ICollection<Payment>> GetAllPayments()
        {
            var PaymentList = await _db.Payment.OrderBy(p => p.CreatedDateTime).ToListAsync();
            return PaymentList;
        }

        public async Task<Payment> GetPaymentById(int Id)
        {
            var PaymentObj = await _db.Payment.SingleOrDefaultAsync(p => p.Id == Id);
            if (PaymentObj == null)
            {
                return null;
            }

            return PaymentObj;
        }

        public async Task<ICollection<Payment>> GetUserPayments(string UserId)
        {
            var PaymentList = await _db.Payment.Where(p=> p.UserId == UserId).OrderBy(p => p.CreatedDateTime).ToListAsync();
            return PaymentList;
        }

        public async Task<ICollection<Payment>> SearchPayment(string UserId, DateTime? FromDate, DateTime? ToDate)
        {
            if(UserId != null && FromDate != null && ToDate != null)
            {
                return await _db.Payment.Where(p => p.UserId == UserId && p.CreatedDateTime.Date >= FromDate && p.CreatedDateTime.Date <= ToDate).OrderBy(p => p.CreatedDateTime).ToListAsync();

            }
            else if(UserId == null && FromDate != null && ToDate != null)
            {
                return await _db.Payment.Where(p => p.CreatedDateTime.Date >= FromDate && p.CreatedDateTime.Date <= ToDate).OrderBy(p => p.CreatedDateTime).ToListAsync();

            }
            else if (UserId != null && FromDate != null && ToDate == null)
            {
                return await _db.Payment.Where(p => p.UserId == UserId && p.CreatedDateTime.Date >= FromDate).OrderBy(p => p.CreatedDateTime).ToListAsync();
            }
            else if (UserId != null && FromDate == null && ToDate != null)
            {
                return await _db.Payment.Where(p => p.UserId == UserId  && p.CreatedDateTime.Date <= ToDate).OrderBy(p => p.CreatedDateTime).ToListAsync();

            }
   
            else if (UserId == null && FromDate == null && ToDate != null)
            {
                return await _db.Payment.Where(p => p.CreatedDateTime.Date <= ToDate).OrderBy(p => p.CreatedDateTime).ToListAsync();

            }
            else if (UserId == null && FromDate != null && ToDate == null)
            {
                return await _db.Payment.Where(p => p.CreatedDateTime.Date >= FromDate).OrderBy(p => p.CreatedDateTime).ToListAsync();

            }
            else if (UserId != null && FromDate == null && ToDate == null)
            {
                return await _db.Payment.Where(p => p.UserId == UserId).OrderBy(p => p.CreatedDateTime).ToListAsync();

            }
            // all null
            return await _db.Payment.OrderBy(p => p.CreatedDateTime).ToListAsync();

         
        }

        public async Task<bool> UpdatePayment(Payment Payment)
        {
            _db.Payment.Update(Payment);
            return await Save();
        }

        public async Task<bool> UpdatePaymentStatus(int Id, string Status)
        {
            var PaymentObj = _db.Payment.SingleOrDefault(p => p.Id == Id);
            if (PaymentObj == null)
            {
                return false;
            }
            PaymentObj.PaymentStatus = Status;
            _db.Payment.Update(PaymentObj);
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
