using IoT_Kids.Models.LMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.LMS
{
    public interface IQuestionChoiceRepo
    {
        Task<bool> CreateChoice(QuestionChoice Choice);
        Task<ICollection<QuestionChoice>> GetQuestionChoices(int QuestionId);
        Task<QuestionChoice> GetChoiceById(int Id);
        Task<bool> Update(QuestionChoice Choice);
        Task<bool> DeleteChoice(int ChoiceId);
    }
}
