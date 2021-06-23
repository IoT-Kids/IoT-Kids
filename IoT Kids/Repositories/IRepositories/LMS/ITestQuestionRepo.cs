using IoT_Kids.Models.LMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.LMS
{
    public interface ITestQuestionRepo
    {
        Task<bool> CreateQuestion(TestQuestion TestQuestion);
        Task<ICollection<TestQuestion>> GetAllQuestions();
        Task<ICollection<TestQuestion>> GetTestQuestions(int TestId);
        Task<TestQuestion> GetQuestionById(int Id);

        //this function will get last index number +1
        Task<int> GetNewIndex(int TestId);

        Task<bool> UpdateQuestion(TestQuestion TestQuestion);
        Task<bool> DeleteQuestion(int QuestionId);
    }
}
