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
    public class QuestionChoiceController : ControllerBase
    {
        private readonly IQuestionChoiceRepo _choiceRepo;
        private readonly IMapper _mapper;

        public QuestionChoiceController(IQuestionChoiceRepo choiceRepo, IMapper mapper)
        {
            _choiceRepo = choiceRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get  choices of a question
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        [HttpGet("GetQuestionChoices/{QuestionId}", Name = "GetQuestionChoices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQuestionChoices(int QuestionId)
        {
            var ChoiceList = await _choiceRepo.GetQuestionChoices(QuestionId);

            var ChoiceDtoList = new List<QuestionChoiceDto>();

            // Converting to Dto list
            foreach (var obj in ChoiceList)
            {
                ChoiceDtoList.Add(_mapper.Map<QuestionChoiceDto>(obj));
            }

            return Ok(ChoiceDtoList);
        }

        /// <summary>
        /// Get Choice by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetChoiceById/{Id}", Name = "GetChoiceById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetChoiceById(int Id)
        {
            var ChoiceObj = await _choiceRepo.GetChoiceById(Id);

            if (ChoiceObj == null)
            {
                return NotFound();
            }
            var ChoiceObjDto = _mapper.Map<QuestionChoiceDto>(ChoiceObj);
            return Ok(ChoiceObjDto);
        }

        /// <summary>
        /// Create new choice
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreateChoice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateChoice([FromBody] CrUpQuestionChoiceDto ChoiceDto)
        {

            var ChoiceObj = _mapper.Map<QuestionChoice>(ChoiceDto);
            if (!await _choiceRepo.CreateChoice(ChoiceObj))
            {
                return StatusCode(404);
            }

            var NewChoiceDto = _mapper.Map<QuestionChoiceDto>(ChoiceObj);

            return CreatedAtRoute("GetChoiceById", new { Id = NewChoiceDto.Id }, NewChoiceDto);
        }

        /// <summary>
        /// update a choice
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ChoiceDto"></param>
        /// <returns></returns>
        [HttpPatch("UpdateQuestion/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateQuestion(int Id, [FromBody] CrUpQuestionChoiceDto ChoiceDto)
        {
            if (ChoiceDto == null || Id == 0)
            {
                return BadRequest(ModelState);
            }

            var ChoiceObj = _mapper.Map<QuestionChoice>(ChoiceDto);
            ChoiceObj.Id = Id;
            if (!await _choiceRepo.Update(ChoiceObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the question choice");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
        /// <summary>
        /// delete a choice
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteChoice/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteChoice(int Id)
        {
            var ChoiceObj = await _choiceRepo.GetChoiceById(Id);

            if (ChoiceObj == null)
            {
                return NotFound();
            }

            if (!await _choiceRepo.DeleteChoice(Id))
            {
                ModelState.AddModelError("", $"Something went wrong while Deleting the choice");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }

    }
}