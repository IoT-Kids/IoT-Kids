using IoT_Kids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.IUsers
{
    public interface IUserRepo
    {
        Task<bool> RegisterUser(string FullName,  string Email, string Phone, string Address, string Password);

        Task<AppUser> GetUser(string Email);

        Task<AppUser> GetUserbyId(string Id);

        Task<bool> CheckEmailExist(string Email);

        Task<bool> CheckUserExist(string Id);

        Task<ICollection<AppUser>> GetUsers();
        Task<ICollection<AppUser>> GetDeletedUsers();
        Task<bool> UpdateUser(AppUser appUser);

        Task<bool> ResetPassword(AppUser UserObj, string NewPassword);

        Task<bool> DeleteUser(AppUser UserObj);
        Task<bool> UnDeleteUser(AppUser UserObj);

    }
}
