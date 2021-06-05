using IoT_Kids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.IMembershipPlans
{
    public interface IMembershipPlanRepo
    {
        Task<bool> CreatePlan(MembershipPlan MembershipPlan);
        Task<MembershipPlan> GetPlanbyName(string Name);
        Task<MembershipPlan>  GetPlanbyId(int Id);
        Task<bool> CheckPlanExist(string Name);
        Task<ICollection<MembershipPlan>> GetPlans();
        Task<bool> UpdatePlan(MembershipPlan MembershipPlan);
        Task<bool> DeletePlan(MembershipPlan MembershipPlan);
    }
}
