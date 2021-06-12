using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IoT_Kids.Models;
using IoT_Kids.Models.Memberships;
using IoT_Kids.Models.Memberships.Dtos;
using IoT_Kids.Repositories.IRepositories.IMembers;
using IoT_Kids.Repositories.IRepositories.IMembershipPlans;
using IoT_Kids.StaticDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Kids.Controllers.Memberships
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepo _memberRepo;
        private readonly IMembershipPlanRepo _MembershipPlanRepo;
        private readonly IMapper _mapper;
        public MemberController(IMemberRepo memberRepo, IMembershipPlanRepo MembershipPlanRepo, IMapper mapper)
        {
            _memberRepo = memberRepo;
            _mapper = mapper;
            _MembershipPlanRepo = MembershipPlanRepo;
        }

        /// <summary>
        /// Get member(s) by their name(s)
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet("GetMemberByName/{Name}")]
        public async Task<IActionResult> GetMemberByName(string Name)
        {
            // here we are getting data from a VM as we want to join 3 different
            // tables, Members, AppUser, and membership
            var MemberList = await _memberRepo.GetMemberByName(Name);         
            return Ok(MemberList);
        }

        /// <summary>
        /// Get an individual member by email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet("GetMemberbyEmail/{Email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberbyEmail(string Email)
        {
            var MemberObj = await _memberRepo.GetMemberByEmail(Email);

            if (MemberObj == null)
            {
                return NotFound();
            }
            var MemberDto = _mapper.Map<MemberDto>(MemberObj);
            return Ok(MemberDto);
        }

        /// <summary>
        /// Get an individual member by user ID
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet("GetMemberbyUserId/{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberbyUserId(string UserId)
        {
            var MemberObj = await _memberRepo.GetMemberByUserId(UserId);

            if (MemberObj == null)
            {
                return NotFound();
            }
            var MemberDto = _mapper.Map<MemberDto>(MemberObj);
            return Ok(MemberDto);
        }

        /// <summary>
        /// Get all members
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllMembers")]
        public async Task<IActionResult> GetMembers()
        {
            var MemberList = await _memberRepo.GetMembers();
            //var MemberDtoList = new List<MemberDto>();
            //foreach (var obj in MemberList)
            //{
            //    MemberDtoList.Add(_mapper.Map<MemberDto>(obj));
            //}

            return Ok(MemberList);
        }

        /// <summary>
        /// Get an individual member by member ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetMemberbyId/{Id}", Name= "GetMemberbyId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberbyId(int Id)
        {
            var MemberObj = await _memberRepo.GetMemberById(Id);

            if (MemberObj == null)
            {
                return NotFound();
            }
            var MemberDto = _mapper.Map<MemberDto>(MemberObj);
            return Ok(MemberDto);
        }

        /// <summary>
        /// check if the user has active membership and have access or not.
        /// if true, has access. otherwise the member is not active
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet("CheckMemberHasAccess/{UserId}")]
        public async Task<IActionResult> CheckMemberHasAccess(string UserId)
        {
            if(await _memberRepo.CheckMemberExist(UserId))
            {
                return NotFound();
            }
            if(UserId == null)
            {
                return BadRequest();
            }
            // if return true then the member has active membership and can access
          
            return Ok(await _memberRepo.CheckMemberActive(UserId));
        }

        /// <summary>
        /// Create new member
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreateMember")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateMember([FromBody] MemberCreateDto MemberDto)
        {
            //First we need to check here if the user is already a member, if a member then we can not add the user as a member again 
            if (!await _memberRepo.CheckMemberExist(MemberDto.UserId))
            {
                ModelState.AddModelError("", "This user is a member already");
                return StatusCode(400, ModelState);
            }
            // Then we need to get membership Plan detail the user membership to 
            var MembershipPlanObj = await _MembershipPlanRepo.GetPlanbyId(MemberDto.MembershipPlanId);

            var MemberObj = _mapper.Map<Member>(MemberDto);
            MemberObj.StartDateTime = DateTime.Now;
            MemberObj.CreatedDateTime = DateTime.Now;
            // If limited duration set for the selected plan, then we need to get the access period
            if (MembershipPlanObj.HasLimitedDuration)
            {
                var PlanDuration = MembershipPlanObj.Period;
                MemberObj.ExpireDateTime = MemberObj.StartDateTime.AddDays(PlanDuration);
            }

            MemberObj.Status = SD.ActiveMember;

            if(!await _memberRepo.CreateMember(MemberObj))
            {
                ModelState.AddModelError("", $"Something went wrong while creating the member");
                return StatusCode(404, ModelState);
            }
             
           return CreatedAtRoute("GetMemberbyId", new { Id = MemberObj.Id }, MemberObj);
        }

        [HttpPatch("UpdateMember/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMember(int Id, [FromBody] MemberUpdateDto MemberDto)
        {
            if (MemberDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var MemberObj = _mapper.Map<Member>(MemberDto);
 
            if (!await _memberRepo.UpdateMember(MemberObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the member");
                return StatusCode(400, ModelState);
            }
            return Ok();
        }

        /// <summary>
        /// Updates the status of a member
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPatch("UpdateMemberStatus/{UserId}/{Status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMemberStatus(string UserId, string Status)
        {
            if (!await _memberRepo.UpdateMemberStatus(UserId, Status))
            {
                return BadRequest();
            }    
            return Ok();
        }

        /// <summary>
        /// Delete the member found by its own Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMember/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMember(int Id)
        {
            var MemberObj = await _memberRepo.GetMemberById(Id);

            if (MemberObj == null)
            {
                return NotFound();
            }

            if (!await _memberRepo.DeleteMember(Id))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the member");
                return StatusCode(400, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Delete the member found by the UserID
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMemberByUserId/{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMemberByUserId(string UserId)
        {
            var MemberObj = await _memberRepo.GetMemberByUserId(UserId);

            if (MemberObj == null)
            {
                return NotFound();
            }

            if (!await _memberRepo.DeleteMemberByUser(UserId))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the member");
                return StatusCode(400, ModelState);
            }

            return Ok();
        }
    }
}