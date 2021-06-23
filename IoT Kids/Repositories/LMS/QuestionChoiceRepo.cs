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
    public class QuestionChoiceRepo : IQuestionChoiceRepo
    {
        private readonly ApplicationDbContext _db;

        public QuestionChoiceRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateChoice(QuestionChoice Choice)
        {
            Choice.Index = await GetNewIndex(Choice.QuestionId);
            _db.QuestionChoice.Add(Choice);
            return await Save();
        }

        public async Task<bool> DeleteChoice(int ChoiceId)
        {
            var ChoiceObj = await _db.QuestionChoice.SingleOrDefaultAsync(p => p.Id == ChoiceId);
            if (ChoiceObj == null)
            {
                return false;
            }
            _db.QuestionChoice.Remove(ChoiceObj);
            return await Save();
        }

        public async Task<QuestionChoice> GetChoiceById(int Id)
        {
            var ChoiceObj = await _db.QuestionChoice.SingleOrDefaultAsync(p => p.Id == Id);
            return ChoiceObj;
        }

        public async Task<ICollection<QuestionChoice>> GetQuestionChoices(int QuestionId)
        {
            var ChoiceList = await _db.QuestionChoice.Where(p => p.QuestionId == QuestionId).OrderBy(p => p.Index).ToListAsync();
            return ChoiceList;
        }

        public async Task<bool> Update(QuestionChoice Choice)
        {
            var BeforeEdit = _db.QuestionChoice.AsNoTracking().FirstOrDefault(p => p.Id == Choice.Id);
            Choice.Index = BeforeEdit.Index;
            _db.QuestionChoice.Update(Choice);
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<int> GetNewIndex(int QuestionId)
        {
            // Get question obj with lastest index
            var ChoiceObj = await _db.QuestionChoice.Where(p => p.QuestionId == QuestionId)
                .OrderByDescending(p => p.Index).FirstOrDefaultAsync();

            int NewIndex = 1;

            // if no question is created yet, then return index =1
            if (ChoiceObj == null)
            {
                return NewIndex;
            }

            NewIndex = ChoiceObj.Index + 1;

            return NewIndex;

        }
    }
}
