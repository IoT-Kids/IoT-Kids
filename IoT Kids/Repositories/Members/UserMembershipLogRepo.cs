using IoT_Kids.Data;
using IoT_Kids.Models.Memberships;
using IoT_Kids.Repositories.IRepositories.IMembers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.Members
{
    public class UserMembershipLogRepo : IUserMembershipLogRepo
    {
        private readonly ApplicationDbContext _db;

        public UserMembershipLogRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateMembershipLog(UserMembershipLog UserMembershipLog)
        {
             _db.UserMembershipLog.Add(UserMembershipLog);
            return await Save();
        }

        public async Task<bool> DeleteMembershipLog(int Id)
        {
            var MembershipObj = _db.UserMembershipLog.SingleOrDefault(p => p.Id == Id);
            if (MembershipObj == null)
            {
                return false;
            }
            _db.UserMembershipLog.Remove(MembershipObj);
            return await Save();
        }
     
        public async Task<ICollection<UserMembershipLog>> GetAllMemberships()
        {
            var UserMembershipLogs = await _db.UserMembershipLog.OrderBy(p => p.StartDate).ToListAsync();
            return UserMembershipLogs;
        }

        public async Task<ICollection<UserMembershipLog>> GetMemberships(string UserId)
        {
            var MembershipLogList = await _db.UserMembershipLog.Where(p => p.UserId == UserId).ToListAsync();
            return MembershipLogList;
        }
         
        public async Task<bool> UpdateMembershipLog(UserMembershipLog UserMembershipLog)
        {
            _db.UserMembershipLog.Update(UserMembershipLog);
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
