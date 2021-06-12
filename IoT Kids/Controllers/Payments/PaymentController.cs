using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IoT_Kids.AssistingModels;
using IoT_Kids.Models;
using IoT_Kids.Models.Users.Dtos;
using IoT_Kids.Models.Payments;
using IoT_Kids.Models.Payments.Dtos;
using IoT_Kids.Repositories.IRepositories.IMembershipPlans;
using IoT_Kids.Repositories.IRepositories.IUsers;
using IoT_Kids.Repositories.IRepositories.IPayments;
using IoT_Kids.StaticDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Kids.Controllers.Payments
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMembershipPlanRepo _planRepo;
        private readonly ICouponRepo _couponRepo;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentRepo paymentRepo, IUserRepo userRepo, ICouponRepo couponRepo, IMapper mapper, IMembershipPlanRepo planRepo)
        {
            _paymentRepo = paymentRepo;
            _userRepo = userRepo;
            _couponRepo = couponRepo;
            _mapper = mapper;
            _planRepo = planRepo;
        }

        /// <summary>
        /// Get payment by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetPaymentById/{Id}", Name = "GetPaymentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentById(int Id)
        {
            var PaymentObj = await _paymentRepo.GetPaymentById(Id);

            if (PaymentObj == null)
            {
                return NotFound();
            }
            var PaymentDto = _mapper.Map<PaymentDto>(PaymentObj);
            return Ok(PaymentDto);
        }

        /// <summary>
        /// Get user's payments (previous payments)
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet("GetUserPayments/{UserId}")]
        public async Task<IActionResult> GetUserPayments(string UserId)
        {
            var PaymentList = await _paymentRepo.GetUserPayments(UserId);

            var PaymentDtoList = new List<PaymentDto>();

            // Converting to Dto list
            foreach (var obj in PaymentList)
            {
                PaymentDtoList.Add(_mapper.Map<PaymentDto>(obj));
            }

            return Ok(PaymentDtoList);
        }

        /// <summary>
        /// Get all payments
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllPayments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var PaymentList = await _paymentRepo.GetAllPayments();

            var PaymentDtoList = new List<PaymentDto>();

            // Converting to Dto list
            foreach (var obj in PaymentList)
            {
                PaymentDtoList.Add(_mapper.Map<PaymentDto>(obj));
            }

            return Ok(PaymentDtoList);
        }
        /// <summary>
        /// This API does not work with Swagger. search payment based on userId from and to date
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchPayments")]
        public async Task<IActionResult> SearchPayments([FromBody] SearchParamVM SearchObj)
        {
            var PaymentList = await _paymentRepo.SearchPayment(SearchObj.UserId, SearchObj.FromDate, SearchObj.ToDate);

            var PaymentDtoList = new List<PaymentDto>();

            // Converting to Dto list
            foreach (var obj in PaymentList)
            {
                PaymentDtoList.Add(_mapper.Map<PaymentDto>(obj));
            }

            return Ok(PaymentDtoList);
        }


        [HttpPost("CreatePayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDto PaymentDto)
        {
            var PlanObj = await _planRepo.GetPlanbyId(PaymentDto.MembershipPlanId);
            var UserObj = await _userRepo.GetUserbyId(PaymentDto.UserId);

            if (PlanObj == null || UserObj == null)
            {
                return BadRequest();
            }

            Payment PaymentObj; //= new Payment();
            PaymentObj = _mapper.Map<Payment>(PaymentDto);
            PaymentObj.UserAddress = UserObj.Address;
            PaymentObj.UnitPrice = PlanObj.Price;
            PaymentObj.Currency = PlanObj.Currency;
            PaymentObj.PaymentStatus = SD.SuccessfulPayment;
            PaymentObj.CreatedDateTime = DateTime.Now;
            PaymentObj.TotalNetAmt = PlanObj.Price;
            PaymentObj.DiscountAmt = 0;

            if (PaymentDto.CouponId > 0)
            {
                var CouponObj = await _couponRepo.GetCouponById(PaymentDto.CouponId ?? 0) ;
                if(CouponObj != null)
                {
                    PaymentObj.DiscountAmt = CouponObj.DiscountAmt;
                }
            }

            PaymentObj.TotalNetAmt = PlanObj.Price - PaymentObj.DiscountAmt ?? 0;


            if (! await _paymentRepo.CreatePayment(PaymentObj))
            {
                return StatusCode(404);
            }


             return CreatedAtRoute("GetPaymentById", new { Id = PaymentObj.Id }, PaymentObj);
        }

        /// <summary>
        /// Update a Payment
        /// </summary>
        /// <param name="PaymentDto"></param>
        /// <returns></returns>
        //[HttpPatch("UpdatePayment")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult UpdatePayment([FromBody] PaymentUpdateDto PaymentDto)
        //{
        //    if (PaymentDto == null)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var PaymentObj = _mapper.Map<Payment>(PaymentDto);
        //    if (!_paymentRepo.UpdatePayment(PaymentObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong while updating the payment");
        //        return StatusCode(500, ModelState);
        //    }

        //    return Ok();
        //}

        /// <summary>
        /// Update a Payment's status
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPatch("UpdatePaymentStatus/{Id}/{Status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePaymentStatus(int Id, string Status)
        {
            if (Id <= 0 || Status == null)
            {
                return BadRequest(ModelState);
            }
            if (!await _paymentRepo.UpdatePaymentStatus(Id, Status))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the payment");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// delete a payment
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeletePayment/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePayment(int Id)
        {
            var PaymentObj = await _paymentRepo.GetPaymentById(Id);

            if (PaymentObj == null)
            {
                return NotFound();
            }

            if (! await _paymentRepo.DeletePayment(PaymentObj))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the payment");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

    }
}