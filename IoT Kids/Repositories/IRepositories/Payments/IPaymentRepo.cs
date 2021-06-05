using IoT_Kids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.Payments
{
    public interface IPaymentRepo
    {
        Task<bool> CreatePayment(Payment Payment);
        Task<Payment> GetPaymentById(int Id);
        Task<ICollection<Payment>> GetUserPayments(string UserId);
        Task<ICollection<Payment>> GetAllPayments();
        Task<ICollection<Payment>> SearchPayment(string UserId, DateTime? FromDate, DateTime? ToDate);
        // Payment GetPaymentByOrderNo(int OrderNo);
        Task<bool> UpdatePayment(Payment Payment);
        Task<bool> UpdatePaymentStatus(int Id, string status);
        Task<bool> DeletePayment(Payment Payment);
 
    }
}
