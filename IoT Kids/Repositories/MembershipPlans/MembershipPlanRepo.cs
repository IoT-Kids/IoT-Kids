using IoT_Kids.Data;
using IoT_Kids.Models;
using IoT_Kids.Models.Memberships;
using IoT_Kids.Repositories.IRepositories;
using IoT_Kids.Repositories.IRepositories.IMembershipPlans;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.MembershipPlans
{
    public class MembershipPlanRepo : IMembershipPlanRepo
    {
        private readonly ApplicationDbContext _db;

        public MembershipPlanRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CheckPlanExist(string Name)
        {
                var Plan = await _db.MembershipPlan.FirstOrDefaultAsync(p => p.PlanName == Name);
                if (Plan == null)
                {
                    return true;
                }
                return false;
         
        }

        public async Task<bool> CreatePlan(MembershipPlan MembershipPlan)
        {
            _db.MembershipPlan.Add(MembershipPlan);
            return await Save();
        }

        public async Task<bool> DeletePlan(MembershipPlan PlanObj)
        {
            //var Plan = await _db.MembershipPlan.SingleOrDefaultAsync(p => p.Id == Id);
            //if(Plan == null)
            //{
            //    return false;
            //}
            _db.MembershipPlan.Remove(PlanObj);
            return await Save();
        }

        public async Task<MembershipPlan> GetPlanbyId(int Id)
        {
            var Plan = await _db.MembershipPlan.SingleOrDefaultAsync(p => p.Id == Id);
            
            return Plan;
        }

        public async Task<MembershipPlan> GetPlanbyName(string Name)
        {
            var Plan = await _db.MembershipPlan.FirstOrDefaultAsync(p => p.PlanName == Name);
            return Plan;
        }

        public async Task<ICollection<MembershipPlan>> GetPlans()
        {
            var PlanList = await _db.MembershipPlan.OrderBy(p=>p.PlanName).ToListAsync();
            return PlanList;
        }

        public async Task<bool> UpdatePlan(MembershipPlan MembershipPlan)
        {
             var CreatedDateTime = _db.MembershipPlan.AsNoTracking().FirstOrDefault(p => p.Id == MembershipPlan.Id).CreatedDateTime;
 
            MembershipPlan.CreatedDateTime = CreatedDateTime;
            _db.MembershipPlan.Update(MembershipPlan);
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

    }
}
