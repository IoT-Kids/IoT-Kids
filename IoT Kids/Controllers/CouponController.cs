using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IoT_Kids.Models;
using IoT_Kids.Models.Dtos;
using IoT_Kids.Repositories.IRepositories.Payments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IoT_Kids.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase 
    {
        private readonly ICouponRepo _CouponRepo;
        private readonly IMapper _mapper;

        public CouponController(ICouponRepo CouponRepo, IMapper mapper)
        {
            _CouponRepo = CouponRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Coupon by code
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpGet("GetCouponByCode/{Code}", Name = "GetCouponByCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCouponByCode(string Code)
        {
            var CouponObj = await _CouponRepo.GetCouponByCode(Code);

            if (CouponObj == null)
            {
                return NotFound();
            }
            var CouponObjDto = _mapper.Map<CouponDto>(CouponObj);
            return Ok(CouponObjDto);
        }

        /// <summary>
        /// Get coupon by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetCouponById/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCouponById(int Id)
        {
            var CouponObj = await _CouponRepo.GetCouponById(Id);

            if (CouponObj == null)
            {
                return NotFound();
            }
            var CouponObjDto = _mapper.Map<CouponDto>(CouponObj);
            return Ok(CouponObjDto);
        }

        /// <summary>
        /// check if coupon active by ID 
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpGet("CheckCouponActive/{Code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckCouponActive(string Code)
        {
            var CouponObj = await _CouponRepo.CheckCouponActive(Code);

            if (CouponObj == false)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// check if coupon active by Code 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("CheckCouponActiveById/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CheckCouponActiveById(int Id)
        {
            var CouponObj = await _CouponRepo.CheckCouponActiveById(Id);

            if (CouponObj == false)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Get all coupons
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllCopons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCopons()
        {
            var CouponList = await _CouponRepo.GetAllCopons();

            var CouponDtoList = new List<CouponDto>();

            // Converting to Dto list
            foreach (var obj in CouponList)
            {
                CouponDtoList.Add(_mapper.Map<CouponDto>(obj));
            }

            return Ok(CouponDtoList);
        }

        /// <summary>
        /// Create new coupon
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreateCoupon")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateCoupon([FromBody] CouponCreateDto CouponDto)
        {
            // we need to check here if the email is used before, emails can not be duplicated 
            if (! await _CouponRepo.CheckCouponCodeExists(CouponDto.Code))
            {
                ModelState.AddModelError("", "This coupon code exists");
                return StatusCode(404, ModelState);
            }
            var CouponObj = _mapper.Map<Coupon>(CouponDto);
            CouponObj.CreatedDateTime = DateTime.Now;
            if(!await _CouponRepo.CreateCoupon(CouponObj))
            {
                return StatusCode(404);
            }

            var NewCouponDto = _mapper.Map<CouponDto>(CouponObj);

            return CreatedAtRoute("GetCouponByCode", new { Code = NewCouponDto.Code }, NewCouponDto);
        }

        /// <summary>
        /// update a coupon
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CouponDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateCoupon/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCoupon(int Id, [FromBody] CouponUpdateDto CouponDto)
        {
            if (CouponDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var CouponObj = _mapper.Map<Coupon>(CouponDto);
            if(!await _CouponRepo.UpdateCoupon(CouponObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the coupon");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// delete a coupon
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCoupon/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCoupon(int Id)
        {
            var CouponObj = await _CouponRepo.GetCouponById(Id);

            if (CouponObj == null)
            {
                return NotFound();
            }

            if (! await _CouponRepo.DeleteCoupon(Id))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the coupon");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

     
    }
}