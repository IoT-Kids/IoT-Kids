using IoT_Kids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.Payments
{
    public interface ICouponRepo
    {
        Task<bool> CreateCoupon(Coupon Coupon);
        Task<ICollection<Coupon>> GetAllCopons();
        Task<bool> UpdateCoupon(Coupon Coupon);
        Task<bool> DeleteCoupon(int Id);
        Task<Coupon> GetCouponById(int Id);
        Task<bool> CheckCouponCodeExists(string Code);
        Task<Coupon> GetCouponByCode(string Code);
        Task<bool> CheckCouponActive(string Code);
        Task<bool> CheckCouponActiveById(int Id);
        Task<Coupon> GetCouponActiveById(int Id);
    }
}
