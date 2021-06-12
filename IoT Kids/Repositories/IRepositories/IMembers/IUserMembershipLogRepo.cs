using IoT_Kids.Models.Memberships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.IMembers
{
    public interface IUserMembershipLogRepo
    {
        Task<bool> CreateMembershipLog(UserMembershipLog UserMembershipLog);
        Task<ICollection<UserMembershipLog>> GetMemberships(string UserId);
        Task<ICollection<UserMembershipLog>> GetAllMemberships();
        Task<bool> UpdateMembershipLog(UserMembershipLog UserMembershipLog);
        Task<bool> DeleteMembershipLog(int Id);
    }
}
