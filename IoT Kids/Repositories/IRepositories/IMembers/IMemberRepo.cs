using IoT_Kids.AssistingModels;
using IoT_Kids.Models.Memberships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace IoT_Kids.Repositories.IRepositories.IMembers
{
    public interface IMemberRepo
    {
        Task<bool> CreateMember(Member Member);
        Task<ICollection<MemberUserPlanVM>> GetMemberByName(string Name);
        Task<Member> GetMemberByEmail(string Email);
        Task<Member> GetMemberByUserId(string UserId);
        Task<Member> GetMemberById(int Id);
        Task<bool> CheckMemberExist(string UserId);
        Task<bool> CheckMemberActive(string UserId);
        Task<ICollection<MemberUserPlanVM>> GetMembers();
        Task<bool> SetStatusExpired(/*System.Threading.CancellationToken stoppingToken*/);
        Task<bool> UpdateMember(Member Member);
        Task<bool> UpdateMemberStatus(string UserId, string status);
        Task<bool> DeleteMemberByUser(string UserId);
        Task<bool> DeleteMember(int Id);
    }
}
