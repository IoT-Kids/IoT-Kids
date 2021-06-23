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
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepo _lessonRepo;
        private readonly IMapper _mapper;

        public LessonController(ILessonRepo LessonRepo, IMapper mapper)
        {
            _lessonRepo = LessonRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Lesson by title
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet("GetLessonByTitle/{Title}", Name = "GetLessonByTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLessonByTitle(string Title)
        {
            var LessonList = await _lessonRepo.GetLessonByTitle(Title);

            var LessonDtoList = new List<LessonDto>();

            // Converting to Dto list
            foreach (var obj in LessonList)
            {
                LessonDtoList.Add(_mapper.Map<LessonDto>(obj));
            }

            return Ok(LessonDtoList);


        }
        /// <summary>
        /// Get  Lessons of a course
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        [HttpGet("GetCourseLessons/{CourseId}", Name = "GetCourseLessons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseLessons(int CourseId)
        {
            var LessonList = await _lessonRepo.GetCourseLessons(CourseId);

            var LessonDtoList = new List<LessonDto>();

            // Converting to Dto list
            foreach (var obj in LessonList)
            {
                LessonDtoList.Add(_mapper.Map<LessonDto>(obj));
            }

            return Ok(LessonDtoList);


        }

        /// <summary>
        /// Get Lesson by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetLessonById/{Id}", Name = "GetLessonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLessonById(int Id)
        {
            var LessonObj = await _lessonRepo.GetLessonById(Id);

            if (LessonObj == null)
            {
                return NotFound();
            }
            var CouponObjDto = _mapper.Map<LessonDto>(LessonObj);
            return Ok(CouponObjDto);
        }

        /// <summary>
        /// Get all Lessons
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllLessons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLessons()
        {
            var LessonList = await _lessonRepo.GetAllLessons();

            var LessonDtoList = new List<LessonDto>();

            // Converting to Dto list
            foreach (var obj in LessonList)
            {
                LessonDtoList.Add(_mapper.Map<LessonDto>(obj));
            }

            return Ok(LessonDtoList);
        }

        /// <summary>
        /// Create new Lesson
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreateLesson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateLesson([FromBody] CrUpLessonDto LessonDto)
        {

            var LessonObj = _mapper.Map<Lesson>(LessonDto);
            LessonObj.CreatedDateTime = DateTime.Now;
            if (!await _lessonRepo.CreateLesson(LessonObj))
            {
                return StatusCode(404);
            }

            var NewLessonDto = _mapper.Map<LessonDto>(LessonObj);

            return CreatedAtRoute("GetLessonById", new { Id = NewLessonDto.Id }, NewLessonDto);
        }

        /// <summary>
        /// update a Lesson
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="LessonDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateLesson/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLesson(int Id, [FromBody] CrUpLessonDto LessonDto)
        {
            if (LessonDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var LessonObj = _mapper.Map<Lesson>(LessonDto);
            LessonObj.Id = Id;
            if (!await _lessonRepo.UpdateLesson(LessonObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the Lesson");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Updates the status of the Lesson
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPatch("UpdateLessonStatus/{Id}/{Status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLessonStatus(int Id, string Status)
        {
            if (!await _lessonRepo.UpdateLessonStatus(Id, Status))
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// delete a Lesson
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteLesson/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLesson(int Id)
        {
            var LessonObj = await _lessonRepo.GetLessonById(Id);

            if (LessonObj == null)
            {
                return NotFound();
            }

            if (!await _lessonRepo.DeleteLesson(Id))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the coupon");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }
    }
}