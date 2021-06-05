using IoT_Kids.AssistingModels;
using IoT_Kids.Data;
using IoT_Kids.Models;
using IoT_Kids.Repositories.IRepositories.IMembers;
using IoT_Kids.StaticDetails;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.Members
{
    public class MemberRepo : IMemberRepo
    {
        private readonly ApplicationDbContext _db;

        public MemberRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        // this function will check if the user already has been added as a member.
        // the function will be used when creating new member
        public async Task<bool> CheckMemberExist(string UserId)
        {
            var MemberObj = await _db.Member.FirstOrDefaultAsync(p => p.UserId == UserId);
            if (MemberObj == null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateMember(Member Member)
        {
            _db.Member.Add(Member);
            return await Save();
        }

        // deleted the member which is found by user id 
        public async Task<bool> DeleteMemberByUser(string UserId)
        {
            var MemberObj =  _db.Member.SingleOrDefault(p => p.UserId == UserId);
            if (MemberObj == null)
            {
                return false;
            }
            _db.Member.Remove(MemberObj);
            return await Save();
        }

        // deleted the member which is found by its id 
        public async Task<bool> DeleteMember(int Id)
        {
            var MemberObj = _db.Member.SingleOrDefault(p => p.Id == Id);
            if (MemberObj == null)
            {
                return false;
            }
            _db.Member.Remove(MemberObj);
            return await Save();
        }


        public async Task<Member> GetMemberByEmail(string Email)
        {
            var MemberObj = await _db.Member.FirstOrDefaultAsync(p => p.AppUser.Email.Contains(Email.ToLower().ToString()));
            if (MemberObj == null)
            {
                return null;
            }
            return MemberObj;
        }

        // returning the member object by user id
        public async Task<Member> GetMemberByUserId(string UserId)
        {
            var MemberObj = await _db.Member
                .Include(p=>p.AppUser)
                .Include(p=>p.MembershipPlan)
                .SingleOrDefaultAsync(p => p.UserId == UserId);
            if (MemberObj == null)
            {
                return null;
            }

            return MemberObj;

        }

        // returning the member object by member id
        public async Task<Member> GetMemberById(int Id)
        {
            var MemberObj = await _db.Member.Include(p=>p.AppUser).Include(p=>p.MembershipPlan).SingleOrDefaultAsync(p => p.Id == Id);
            if (MemberObj == null)
            {
                return null;
            }
            return MemberObj;
           
        }

        // returns list of members based on searched name
        // here we will be using VM as we will need to return data from 3 
        // different tables, Member, AppUser, MembershipPlan
        public async Task<ICollection<MemberUserPlanVM>> GetMemberByName(string Name)
        {
            var MemberList = await (from e in _db.Member
                                join mf in _db.MembershipPlan on e.MembershipPlanId equals mf.Id
                                where e.AppUser.FullName.Contains(Name.ToLower().ToString())
                                select new MemberUserPlanVM
                                {
                                    MemberId = e.Id,
                                    FullName = e.AppUser.FullName,
                                    PhoneNo = e.AppUser.PhoneNumber,
                                    Email = e.AppUser.Email,
                                    PlanName = mf.PlanName,
                                    StartDateTime = e.StartDateTime.ToString("dd/MM/yyyy"),
                                    ExpireDateTime = e.ExpireDateTime.ToString("dd/MM/yyyy"),
                                    CreatedDateTime = e.CreatedDateTime.ToString("dd/MM/yyyy"),
                                    Status = e.Status
                                }).ToListAsync();
            return MemberList;
        }

        public async Task<ICollection<MemberUserPlanVM>> GetMembers()
        {
            var MemberList = await (from e in _db.Member
                                    join mf in _db.MembershipPlan on e.MembershipPlanId equals mf.Id
                                   // where e.AppUser.FullName.Contains(Name.ToLower().ToString())
                                    select new MemberUserPlanVM
                                    {
                                        MemberId = e.Id,
                                        FullName = e.AppUser.FullName,
                                        PhoneNo = e.AppUser.PhoneNumber,
                                        Email = e.AppUser.Email,
                                        PlanName = mf.PlanName,
                                        StartDateTime = e.StartDateTime.ToString("dd/MM/yyyy"),
                                        ExpireDateTime = e.ExpireDateTime.ToString("dd/MM/yyyy"),
                                        CreatedDateTime = e.CreatedDateTime.ToString("dd/MM/yyyy"),
                                        Status = e.Status
                                    }).ToListAsync();
            
            // var MemberList = await _db.Member.OrderBy(p => p.AppUser.FullName).ToListAsync();
            return MemberList;
        }

        public async Task<bool> UpdateMember(Member Member)
        {
          //  var x = _db.MembershipPlan.AsNoTracking().FirstOrDefault(p => p.Id == MembershipPlan.Id).CreatedDateTime;

          //  MembershipPlan.CreatedDateTime = x;
            _db.Member.Update(Member);
            return await Save();
        }
 
        public async Task<bool> UpdateMemberStatus(string UserId, string status)
        {
            var MemberObj = _db.Member.SingleOrDefault(p => p.UserId == UserId);
            MemberObj.Status = status;
            return await Save();
        }

        // update all members' status to expired if exipred date = today's date
        // since it is scheduled service, it will run every 24 hour, we have to add cancellationToken
        public async Task<bool> SetStatusExpired(/*System.Threading.CancellationToken stoppingToken*/)
        {
            //get list of members who's expiry date = today's date 
            var ExpiredMembers = _db.Member.Where(p => p.ExpireDateTime.Date <= DateTime.Now.Date 
            && p.Status != SD.ExpiredMember);
            
            foreach(var MemberObj in ExpiredMembers)
            {
                MemberObj.Status = SD.ExpiredMember;
            }  
            return await Save();
        }

        // this function will check if the user's membership is active
        // so that the user will be granted an access or not based on membership status
        public async Task<bool> CheckMemberActive(string UserId)
        {
            var MemberObj = await _db.Member.SingleOrDefaultAsync(p => p.UserId == UserId);
            if(MemberObj.Status == SD.ActiveMember)
            {
                return true;
            }
            return false;
           
        }

        public async Task<bool> Save()
        {
            try
            {
                return await _db.SaveChangesAsync() >= 0 ? true : false;
            }
            catch
            {
                return false;
            }
            
        }

    }
}
