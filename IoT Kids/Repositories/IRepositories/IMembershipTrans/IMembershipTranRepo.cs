using IoT_Kids.AssistingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.IMembershipTrans
{
    public interface IMembershipTranRepo
    {
        Task<bool> CreateUserMembership(UserMembershipVM UserMembership);
        Task<bool> UpdateUserMembership(UserMembershipVM UserMembership);
    }
}
