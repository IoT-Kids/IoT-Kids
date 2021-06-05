using IoT_Kids.Data;
using IoT_Kids.Models;
using IoT_Kids.Repositories.IRepositories.Payments;
using IoT_Kids.StaticDetails;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.Payments
{
    public class CouponRepo : ICouponRepo
    {
        private readonly ApplicationDbContext _db;

        public CouponRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        // check if the applied coupon is active or not
        // to be active its code must exists, status must be active and exiry date less than today's date
        public async Task<bool> CheckCouponActive(string Code)
        {
            var  CouponObj = await _db.Coupon.FirstOrDefaultAsync(c => c.Code == Code && c.Status == SD.ActiveCoupon);

            if (CouponObj != null)
            {
                // if the coupon has already expired
                if (CouponObj.ExpiryDate != null && CouponObj.ExpiryDate > DateTime.Now.Date)
                {
                    return false; // is expired
                }
            
                return true; // is valid and not expired
            }
            return false; // is not valid 
        }

        public async Task<bool> CheckCouponActiveById(int Id)
        {
            //&& c.ExpiryDate.Date < DateTime.Now.Date
            var CouponObj = await _db.Coupon.FirstOrDefaultAsync(c => c.Id ==Id && c.Status == SD.ActiveCoupon );
            if (CouponObj != null)
            {
                // if the coupon has already expired
                if (CouponObj.ExpiryDate != null && CouponObj.ExpiryDate > DateTime.Now.Date)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        // get a coupon if active by Id 
        public async Task<Coupon> GetCouponActiveById(int Id)
        {
            //&& c.ExpiryDate.Date < DateTime.Now.Date
            var CouponObj = await _db.Coupon.FirstOrDefaultAsync(c => c.Id == Id && c.Status == SD.ActiveCoupon);
            return CouponObj;
        }


        public async Task<bool> CreateCoupon(Coupon Coupon)
        {
            _db.Coupon.Add(Coupon);
            return await Save();
        }

        public async Task<bool> DeleteCoupon(int Id)
        {
            var CouponObj = _db.Coupon.SingleOrDefault(p => p.Id == Id);
            if (CouponObj == null)
            {
                return false;
            }
            _db.Coupon.Remove(CouponObj);
            return await Save();
        }

        public async Task<ICollection<Coupon>> GetAllCopons()
        {
            var CouponList = await _db.Coupon.OrderBy(p => p.Code).ToListAsync();
            return CouponList;
        }

        public async Task<Coupon> GetCouponById(int Id)
        {
            var CouponObj = await _db.Coupon.SingleOrDefaultAsync(p => p.Id == Id);

            return CouponObj;
        }
        public async Task<Coupon> GetCouponByCode(string Code)
        {
            var CouponObj = await _db.Coupon.SingleOrDefaultAsync(p => p.Code == Code);

            return CouponObj;
        }

        public async Task<bool> UpdateCoupon(Coupon Coupon)
        {
            var Datetime = _db.Coupon.AsNoTracking().FirstOrDefault(p => p.Id == Coupon.Id).CreatedDateTime;
            Coupon.CreatedDateTime = Datetime;
            _db.Coupon.Update(Coupon);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> CheckCouponCodeExists(string Code)
        {
            var CouponObj = await _db.Coupon.FirstOrDefaultAsync(c => c.Code == Code);
            if (CouponObj == null) // then it does not exist
            {
                return true;
            }
            return false;
        }
    }
}
