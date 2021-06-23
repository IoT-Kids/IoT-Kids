using IoT_Kids.Data;
using IoT_Kids.Models.LMS;
using IoT_Kids.Repositories.IRepositories.LMS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.LMS
{
    public class TestQuestionRepo : ITestQuestionRepo
    {
        private readonly ApplicationDbContext _db;

        public TestQuestionRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateQuestion(TestQuestion TestQuestion)
        {
            // getting the latest index first 
            TestQuestion.Index =  await GetNewIndex(TestQuestion.TestId);
            _db.TestQuestion.Add(TestQuestion);
            return await Save();
        }

        public async Task<bool> DeleteQuestion(int QuestionId)
        {
            var QuestionObj = await _db.TestQuestion.SingleOrDefaultAsync(p => p.Id == QuestionId);
            if (QuestionObj == null)
            {
                return false;
            }
            _db.TestQuestion.Remove(QuestionObj);
            return await Save();
        }

        public async Task<ICollection<TestQuestion>> GetAllQuestions()
        {
            var QuestionList = await _db.TestQuestion.OrderBy(p => p.CreatedDateTime).ToListAsync();
            return QuestionList;
        }

        public async Task<TestQuestion> GetQuestionById(int Id)
        {
            var QuestionObj = await _db.TestQuestion.SingleOrDefaultAsync(p => p.Id == Id);
            return QuestionObj;
        }

        public async Task<ICollection<TestQuestion>> GetTestQuestions(int TestId)
        {
            var QuestionList = await _db.TestQuestion.Where(p => p.TestId == TestId).OrderBy(p => p.Index).ToListAsync();
            return QuestionList;
        }

        public async Task<bool> UpdateQuestion(TestQuestion TestQuestion)
        {
            var BeforeEdit = _db.TestQuestion.AsNoTracking().FirstOrDefault(p => p.Id == TestQuestion.Id);
            TestQuestion.CreatedDateTime = BeforeEdit.CreatedDateTime;
            TestQuestion.Index = BeforeEdit.Index;
            _db.TestQuestion.Update(TestQuestion);
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<int> GetNewIndex(int TestId)
        {
            // Get question obj with lastest index
            var QuestionObj = await _db.TestQuestion.Where(p => p.TestId == TestId)
                .OrderByDescending(p=>p.Index).FirstOrDefaultAsync();

            int NewIndex = 1;

            // if no question is created yet, then return index =1
            if (QuestionObj == null)
            {
                return NewIndex;
            }

              NewIndex = QuestionObj.Index + 1;

            return NewIndex;

        }
    }
}
