using IoT_Kids.Data;
using IoT_Kids.Models.Users;
using IoT_Kids.Repositories.IRepositories.IUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.Users
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _db;
        public UserRepo(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<bool> RegisterUser(string fullName, string email, string phone, string Address, string password)
        {
            var Appuser = new AppUser { FullName = fullName, UserName = email, Email = email, PhoneNumber = phone, Address = Address, EmailConfirmed = true};
            var result = await _userManager.CreateAsync(Appuser, password);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<AppUser> GetUser(string email)
        {
            var user = await _db.AppUser.SingleOrDefaultAsync(u => u.Email == email);
          
            return user;
        }

        // this function to check if email is already registered before
        public async Task<bool> CheckEmailExist(string email)
        {
            var check = await _db.AppUser.SingleOrDefaultAsync(u => u.Email == email);

            if(check != null)
            {
                return false;
            }

            return true;
        }

        public async Task<AppUser> GetUserbyId(string Id)
        {
            var user = await _db.AppUser.SingleOrDefaultAsync(u => u.Id == Id);
            return user;
        }

        // get all users
        public async Task<ICollection<AppUser>> GetUsers()
        {
            return await _db.AppUser.Where(u=> u.Deleted != true).OrderBy(u=> u.FullName).ToListAsync();
        }

        // get all deleted users
        public async Task<ICollection<AppUser>> GetDeletedUsers()
        {
            return await _db.AppUser.Where(u => u.Deleted == true).OrderBy(u => u.FullName).ToListAsync();
        }

        public async Task<bool> UpdateUser(AppUser appUser)
        {
            var user =  _db.AppUser.SingleOrDefault(u => u.Id == appUser.Id);
            user.FullName = appUser.FullName;
            user.Address = appUser.Address;
            user.Email = appUser.Email;
            user.UserName = appUser.Email;
            user.PhoneNumber = appUser.PhoneNumber;
            
            //_userManager.passwo
             var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteUser(AppUser UserObj)
        {
            UserObj.Deleted = true;
            return await Save();
        }

        public async Task<bool> UnDeleteUser(AppUser UserObj)
        {
            UserObj.Deleted = false;
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> CheckUserExist(string Id)
        {
            var check = await _db.AppUser.SingleOrDefaultAsync(u => u.Id == Id);

            if (check != null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ResetPassword(AppUser UserObj, string NewPassword)
        {
            //var user = _db.AppUser.SingleOrDefault(u => u.Id == Id);
            //if(user == null)
            //{
            //    return false;
            //}
            string token = await _userManager.GeneratePasswordResetTokenAsync(UserObj);
            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(UserObj, token, NewPassword);
            
            if(passwordChangeResult.Succeeded)
            {
                return true;
            }
            return false;
            
        }
    }
}
