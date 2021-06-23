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
    public class TestController : ControllerBase
    {
        private readonly ITestRepo _testRepo;
        private readonly IMapper _mapper;

        public TestController(ITestRepo testRepo, IMapper mapper)
        {
            _testRepo = testRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Test by title
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        [HttpGet("GetTestByTitle/{Title}", Name = "GetTestByTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTestByTitle(string Title)
        {
            var TestList = await _testRepo.GetTestByTitle(Title);

            var TestDtoList = new List<TestDto>();

            // Converting to Dto list
            foreach (var obj in TestList)
            {
                TestDtoList.Add(_mapper.Map<TestDto>(obj));
            }

            return Ok(TestDtoList);


        }
        /// <summary>
        /// Get  Tests of a course
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        [HttpGet("GetCourseTests/{CourseId}", Name = "GetCourseTests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseTests(int CourseId)
        {
            var TestList = await _testRepo.GetCourseTests(CourseId);

            var TestDtoList = new List<TestDto>();

            // Converting to Dto list
            foreach (var obj in TestList)
            {
                TestDtoList.Add(_mapper.Map<TestDto>(obj));
            }

            return Ok(TestDtoList);


        }

        /// <summary>
        /// Get Test by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetTestById/{Id}", Name = "GetTestById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTestById(int Id)
        {
            var TestObj = await _testRepo.GetTestById(Id);

            if (TestObj == null)
            {
                return NotFound();
            }
            var TestObjDto = _mapper.Map<TestDto>(TestObj);
            return Ok(TestObjDto);
        }

        /// <summary>
        /// Get all Tests
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllTests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTests()
        {
            var TestList = await _testRepo.GetAllTests();

            var TestDtoList = new List<TestDto>();

            // Converting to Dto list
            foreach (var obj in TestList)
            {
                TestDtoList.Add(_mapper.Map<TestDto>(obj));
            }

            return Ok(TestDtoList);
        }

        /// <summary>
        /// Create new Test
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreateTest")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateTest([FromBody] CrUpTestDto TestDto)
        {
            if(TestDto.Type == 1 && TestDto.LessonId == null) // in case its mid test and not assigned to a lesson, return error
            {
                ModelState.AddModelError("", $"Lesson can not be empty, this is a mid test!");
                return BadRequest(ModelState);
            }
            var TestObj = _mapper.Map<Test>(TestDto);
            TestObj.CreatedDateTime = DateTime.Now;
            if (!await _testRepo.CreateTest(TestObj))
            {
                return StatusCode(404);
            }

            var NewTestDto = _mapper.Map<TestDto>(TestObj);

            return CreatedAtRoute("GetTestById", new { Id = NewTestDto.Id }, NewTestDto);
        }

        /// <summary>
        /// update a Test
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="TestDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateTest/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTest(int Id, [FromBody] CrUpTestDto TestDto)
        {
            if (TestDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var TestObj = _mapper.Map<Test>(TestDto);
            TestObj.Id = Id;
            if (!await _testRepo.UpdateTest(TestObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the Test");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// Updates the status of the Test
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPatch("UpdateTestStatus/{Id}/{Status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTestStatus(int Id, string Status)
        {
            if (!await _testRepo.UpdateTestStatus(Id, Status))
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// delete a Test
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTest/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTest(int Id)
        {
            var TestObj = await _testRepo.GetTestById(Id);

            if (TestObj == null)
            {
                return NotFound();
            }

            if (!await _testRepo.DeleteTest(Id))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the Test");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }
    }
}
