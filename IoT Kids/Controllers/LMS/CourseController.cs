using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IoT_Kids.Models.LMS;
using IoT_Kids.Models.LMS.Dtos;
using IoT_Kids.Repositories.IRepositories.LMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Kids.Controllers.LMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepo _courseRepo;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepo CourseRepo, IMapper mapper)
        {
            _courseRepo = CourseRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get course by title
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet("GetCourseByTitle/{Title}", Name = "GetCourseByTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseByTitle(string Title)
        {
            var CourseList = await _courseRepo.GetCourseByTitle(Title);

            var CourseDtoList = new List<CourseDto>();

            // Converting to Dto list
            foreach (var obj in CourseList)
            {
                CourseDtoList.Add(_mapper.Map<CourseDto>(obj));
            }

            return Ok(CourseDtoList);

    
        }

        /// <summary>
        /// Get Course by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetCourseById/{Id}", Name = "GetCourseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseById(int Id)
        {
            var CourseObj = await _courseRepo.GetCourseById(Id);

            if (CourseObj == null)
            {
                return NotFound();
            }
            var CourseObjDto = _mapper.Map<CourseDto>(CourseObj);
            return Ok(CourseObjDto);
        }

        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllCourses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses()
        {
            var CourseList = await _courseRepo.GetAllCourses();

            var CourseDtoList = new List<CourseDto>();

            // Converting to Dto list
            foreach (var obj in CourseList)
            {
                CourseDtoList.Add(_mapper.Map<CourseDto>(obj));
            }

            return Ok(CourseDtoList);
        }

        /// <summary>
        /// Create new course
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreateCourse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateCourse([FromBody] CrUpCourseDto CourseDto)
        {
       
            var CourseObj = _mapper.Map<Course>(CourseDto);
            CourseObj.CreatedDateTime = DateTime.Now;
            if (!await _courseRepo.CreateCourse(CourseObj))
            {
                return StatusCode(404);
            }

            var NewCourseDto = _mapper.Map<CourseDto>(CourseObj);

            return CreatedAtRoute("GetCourseById", new { Id = NewCourseDto.Id }, NewCourseDto);
        }

        /// <summary>
        /// update a course
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CourseDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateCourse/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse(int Id, [FromBody] CrUpCourseDto CourseDto)
        {
            if (CourseDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var CourseObj = _mapper.Map<Course>(CourseDto);
            CourseObj.Id = Id;
            if (!await _courseRepo.UpdateCourse(CourseObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the course");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Updates the status of the course
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPatch("UpdateCourseStatus/{Id}/{Status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourseStatus(int Id, string Status)
        {
            if (!await _courseRepo.UpdateCourseStatus(Id, Status))
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// delete a course
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCourse/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            var CourseObj = await _courseRepo.GetCourseById(Id);

            if (CourseObj == null)
            {
                return NotFound();
            }

            if (!await _courseRepo.DeleteCourse(Id))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the Course");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }



    }
}