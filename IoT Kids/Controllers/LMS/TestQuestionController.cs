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
    public class TestQuestionController : ControllerBase
    {
        private readonly ITestQuestionRepo _questionRepo;
        private readonly IMapper _mapper;

        public TestQuestionController(ITestQuestionRepo questionRepo, IMapper mapper)
        {
            _questionRepo = questionRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get  Questions of a test
        /// </summary>
        /// <param name="TestId"></param>
        /// <returns></returns>
        [HttpGet("GetTestQuestions/{TestId}", Name = "GetTestQuestions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTestQuestions(int TestId)
        {
            var QuestionList = await _questionRepo.GetTestQuestions(TestId);

            var TestQuestionDtoList = new List<TestQuestionDto>();

            // Converting to Dto list
            foreach (var obj in QuestionList)
            {
                TestQuestionDtoList.Add(_mapper.Map<TestQuestionDto>(obj));
            }

            return Ok(TestQuestionDtoList);
        }

        /// <summary>
        /// Get Question by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetQuestionById/{Id}", Name = "GetQuestionById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQuestionById(int Id)
        {
            var QuestionObj = await _questionRepo.GetQuestionById(Id);

            if (QuestionObj == null)
            {
                return NotFound();
            }
            var QuestionObjDto = _mapper.Map<TestQuestionDto>(QuestionObj);
            return Ok(QuestionObjDto);
        }

        /// <summary>
        /// Get all Questions
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllQuestions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllQuestions()
        {
            var QuestionList = await _questionRepo.GetAllQuestions();

            var TestQuestionDtoList = new List<TestQuestionDto>();

            // Converting to Dto list
            foreach (var obj in QuestionList)
            {
                TestQuestionDtoList.Add(_mapper.Map<TestQuestionDto>(obj));
            }

            return Ok(TestQuestionDtoList);
        }

        /// <summary>
        /// Create new Question
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreateQuestion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateQuestion([FromBody] CrUpTestQuestionDto TestQuestionDto)
        {

            var QuestionObj = _mapper.Map<TestQuestion>(TestQuestionDto);
            QuestionObj.CreatedDateTime = DateTime.Now;
            if (!await _questionRepo.CreateQuestion(QuestionObj))
            {
                return StatusCode(404);
            }

            var NewTestQuestionDto = _mapper.Map<TestQuestionDto>(QuestionObj);

            return CreatedAtRoute("GetQuestionById", new { Id = NewTestQuestionDto.Id }, NewTestQuestionDto);
        }

        /// <summary>
        /// update a Question
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="TestQuestionDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateQuestion/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateQuestion(int Id, [FromBody] CrUpTestQuestionDto TestQuestionDto)
        {
            if (TestQuestionDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var QuestionObj = _mapper.Map<TestQuestion>(TestQuestionDto);
            QuestionObj.Id = Id;
            if (!await _questionRepo.UpdateQuestion(QuestionObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the Question");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        /// <summary>
        /// delete a Question
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteQuestion/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteQuestion(int Id)
        {
            var QuestionObj = await _questionRepo.GetQuestionById(Id);

            if (QuestionObj == null)
            {
                return NotFound();
            }

            if (!await _questionRepo.DeleteQuestion(Id))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the Question");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

        /// <summary>
        /// Get New Question Index
        /// </summary>
        /// <param name="TestId"></param>
        /// <returns></returns>
        //[HttpGet("GetNewIndex/{TestId}", Name = "GetNewIndex")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetNewIndex(int TestId)
        //{
        //    int QuestionObj = await _questionRepo.GetNewIndex(TestId);

        //    return Ok(QuestionObj);
        //}
    }

}
