using IoT_Kids.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.IMembershipTrans
{
    public interface IMembershipTranRepo
    {
        Task<bool> CreateUserMembership(UserMembershipVM UserMembership);
        Task<bool> RenewUserMembership(UpdateUserMembershipVM UserMembership);
    }
}
