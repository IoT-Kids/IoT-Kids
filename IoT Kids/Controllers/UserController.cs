using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IoT_Kids.Models;
using IoT_Kids.Models.Dtos;
using IoT_Kids.Repositories.IRepositories.IUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Kids.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        public UserController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get an individual user by email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet("GetUserByEmail/{Email}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            var User = await  _userRepo.GetUser(Email);
            
            if (User == null)
            {
                return NotFound();
            }
             var UserDto = _mapper.Map<AppUserDto>(User);

            return Ok(UserDto);
        }

        /// <summary>
        /// Get an individual user by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// [HttpGet("{Id}", Name = "GetUserbyId")]
        [HttpGet("GetUserbyId/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserbyId(string Id)
        {
            var User = await _userRepo.GetUserbyId(Id);

            if (User == null)
            {
                return NotFound();
            }
            var UserDto = _mapper.Map<AppUserDto>(User);
            return Ok(UserDto);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers()
        {
            var UserList = await _userRepo.GetUsers();

            var UserDtoList = new List<AppUserDto>();

            // Converting to Dto list
            foreach (var obj in UserList)
            {
                UserDtoList.Add(_mapper.Map<AppUserDto>(obj));
            }

            return Ok(UserDtoList);
        }

        /// <summary>
        /// This function registers first time users
        /// </summary>
        /// <param name="FullName"></param>
        /// <param name="Email"></param>
        /// <param name="Phone"></param>
        /// <param name="Address"></param>
        /// <param name="Password"></param>
        /// <returns></returns>

        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RegisterUser(string FullName, string Email, string Phone, string Address,string Password)
        {
            // we need to check here if the email is used before, emails can not be duplicated 
            if (!await _userRepo.CheckEmailExist(Email))
            {
                ModelState.AddModelError("", "This email has been registered before");
                return StatusCode(404, ModelState);
            }

            bool NewUser = await _userRepo.RegisterUser(FullName, Email, Phone, Address, Password);

            if (NewUser == false)
            {
                return StatusCode(404);
            }
            var User = await GetUserByEmail(Email);


            return CreatedAtRoute("GetUser", new { Email = Email }, User);
        }

        [HttpPatch("UpdateUser/{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(string UserId, [FromBody] AppUserDto AppUserDto)
        {
            if (AppUserDto == null || UserId != AppUserDto.Id)
            {
                return BadRequest(ModelState);
            }
            var UserObj = _mapper.Map<AppUser>(AppUserDto);
            var Result = await _userRepo.UpdateUser(UserObj);
            if (Result == false)
            {
                ModelState.AddModelError("", $"Something went wrong while updating the user");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
        // the user will not be deleted. but will be marked as deleted 
        /// <summary>
        /// Set user as deleted. No user will be actually deleted here
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPatch("SetUserDeleted/{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetUserDeleted(string UserId)
        {
            var UserObj = await _userRepo.GetUserbyId(UserId);

            if (UserObj == null)
            {
                return NotFound();
            }

            if (!await _userRepo.DeleteUser(UserObj))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the user");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Change user deleted to undeleted 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPatch("SetUserUnDeleted/{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetUserUnDeleted(string UserId)
        {
            var UserObj = await _userRepo.GetUserbyId(UserId);

            if (UserObj == null)
            {
                return NotFound();
            }

            if (!await _userRepo.UnDeleteUser(UserObj))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the user");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// to change users password
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        [HttpPatch("ResetPassword/{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResetPassword(string UserId, string NewPassword)
        {
            var UserObj = await _userRepo.GetUserbyId(UserId);

            if (UserObj == null)
            {
                return NotFound();
            }

            var result =  _userRepo.ResetPassword(UserObj, NewPassword);
            
            return Ok(result);
        }

        /// <summary>
        /// Get all Deleted users
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDeletedUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeletedUsers()
        {
            var UserList = await _userRepo.GetDeletedUsers();

            var UserDtoList = new List<AppUserDto>();

            // Converting to Dto list
            foreach (var obj in UserList)
            {
                UserDtoList.Add(_mapper.Map<AppUserDto>(obj));
            }

            return Ok(UserDtoList);
        }
    }
}