using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IoT_Kids.Models.Dtos;
using IoT_Kids.Repositories.IRepositories.IMembers;
//using IoT_Kids.Repositories.IRepositories.ISubscriptionPlans;
//using IoT_Kids.Repositories.IRepositories.IMembershipPlans;
using IoT_Kids.Repositories.IRepositories.IMembershipTrans;
using IoT_Kids.Repositories.IRepositories.IUsers;
using IoT_Kids.Repositories.IRepositories.Payments;
using IoT_Kids.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Kids.Controllers.Transactions
{
    /// <summary>
    /// this controller handles user membership, either new user or existing member
    /// the controller will also handles failed attempts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipTransController : ControllerBase
    {
        private readonly IMembershipTranRepo _MembershipTransRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public MembershipTransController(IMembershipTranRepo MembershipTransRepo, 
            IUserRepo userRepo,  IMapper mapper)
        {
            _MembershipTransRepo = MembershipTransRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Create   Membership for a first time user
        /// </summary>
        /// <param name="UserMembership"></param>
        /// <returns></returns>
        [HttpPost("CreateMembership")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateMembership([FromBody] UserMembershipVM UserMembership)
        {
            if (! await _userRepo.CheckEmailExist(UserMembership.Email))
            {
                return BadRequest("This email already exists");
            }
            if (!_MembershipTransRepo.CreateUserMembership(UserMembership).GetAwaiter().GetResult())
            {
                return BadRequest();
            }

            var NewUser = await _userRepo.GetUser(UserMembership.Email);

            var UserDto = _mapper.Map<AppUserDto>(NewUser);
            return Ok(UserDto);
        }

        // Membership renewal with new payment 
        /// <summary>
        /// Update Membership for an existing user
        /// </summary>
        /// <param name="UserMembership"></param>
        /// <returns></returns>
        [HttpPost("UserMembershipRenewal")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UserMembershipRenewal([FromBody] UserMembershipVM UserMembership)
        {
            if (await _userRepo.CheckUserExist(UserMembership.UserId))
            {
                return BadRequest("The user does not exist");
            }

            if (! await _MembershipTransRepo.UpdateUserMembership(UserMembership))
            {
                return BadRequest();
            }

            var UserObj = _userRepo.GetUserbyId(UserMembership.UserId);
            var UserDto = _mapper.Map<AppUserDto>(UserObj);
            return Ok(UserDto);
        }


    }
}