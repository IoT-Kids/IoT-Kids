using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IoT_Kids.Models.Users.Dtos;
using IoT_Kids.Models.Memberships;
using IoT_Kids.Models.Memberships.Dtos;
using IoT_Kids.Repositories.IRepositories;
using IoT_Kids.Repositories.IRepositories.IMembershipPlans;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Kids.Controllers.Memberships
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipPlanController : ControllerBase
    {
        private readonly IMembershipPlanRepo _planRepo;
        private readonly IMapper _mapper;
        public MembershipPlanController(IMembershipPlanRepo planRepo, IMapper mapper)
        {
            _planRepo = planRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get an individual membership by plan name
        /// </summary>
        /// <param name="PlanName"></param>
        /// <returns></returns>
        [HttpGet("GetPlanByName/{PlanName}", Name = "GetPlanByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlanByName(string PlanName)
        {
            var PlanObj = await _planRepo.GetPlanbyName(PlanName);

            if (PlanObj == null)
            {
                return NotFound();
            }
            var PlanDto = _mapper.Map<MembershipPlanDto>(PlanObj);
            return Ok(PlanDto);
        }

        /// <summary>
        /// Get an individual membership plan by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// [HttpGet("{Id}", Name = "GetUserbyId")]
        [HttpGet("GetPlanbyId/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlanbyId(int Id)
        {
            var PlanObj = await _planRepo.GetPlanbyId(Id);

            if (PlanObj == null)
            {
                return NotFound();
            }
            var PlanDto = _mapper.Map<MembershipPlanDto>(PlanObj);
            return Ok(PlanDto);
        }

        /// <summary>
        /// Get all Membership plans
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPlans", Name = "GetPlans")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlans()
        {
            var PlanList = await _planRepo.GetPlans();

            var PlanDtoList = new List<MembershipPlanDto>();

            // Converting to Dto list
            foreach (var Obj in PlanList)
            {
                PlanDtoList.Add(_mapper.Map<MembershipPlanDto>(Obj));
            }

            return Ok(PlanDtoList);
        }

        /// <summary>
        /// Create new plan
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreatePlan")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreatePlan([FromBody] MembershipPlanCreateDto SubPlanDto)
        {
            // we need to check here if the email is used before, emails can not be duplicated 
            if (!await _planRepo.CheckPlanExist(SubPlanDto.PlanName))
            {
                ModelState.AddModelError("", "This plan exists");
                return StatusCode(404, ModelState);
            }
            var PlanObj = _mapper.Map<MembershipPlan>(SubPlanDto);
            PlanObj.CreatedDateTime = DateTime.Now;
       
            if (!await _planRepo.CreatePlan(PlanObj))
            {
                return StatusCode(404);
            }
             var NewSubPlanDto = _mapper.Map<MembershipPlanDto>(PlanObj);

            return CreatedAtRoute("GetPlanByName", new { PlanName = SubPlanDto.PlanName }, NewSubPlanDto);
        }

        [HttpPatch("UpdatePlan/{Id}", Name = "UpdatePlan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlan(int Id, [FromBody] MembershipPlanDto SubPlanDto)
        {
            if (SubPlanDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var PlanObj = _mapper.Map<MembershipPlan>(SubPlanDto);

            if (!await _planRepo.UpdatePlan(PlanObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the plan");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpDelete("DeletePlan/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlan(int Id)
        {
            var PlanObj = await _planRepo.GetPlanbyId(Id);

            if (PlanObj == null)
            {
                return NotFound();
            }

            if (!await _planRepo.DeletePlan(PlanObj))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the plan");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}