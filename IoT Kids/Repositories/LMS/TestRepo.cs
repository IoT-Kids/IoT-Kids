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
    public class TestRepo : ITestRepo
    {
        private readonly ApplicationDbContext _db;

        public TestRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateTest(Test Test)
        {
            _db.Test.Add(Test);
            return await Save();
        }

        public async Task<bool> DeleteTest(int TestId)
        {
            var TestObj = await _db.Test.SingleOrDefaultAsync(p => p.Id == TestId);
            if (TestObj == null)
            {
                return false;
            }
            _db.Test.Remove(TestObj);
            return await Save();
        }

        public async Task<ICollection<Test>> GetAllTests()
        {
            var TestList = await _db.Test.OrderBy(p => p.CreatedDateTime).ToListAsync();
            return TestList;
        }

        public async Task<ICollection<Test>> GetCourseTests(int CourseId)
        {
            var TestList = await _db.Test.Where(p => p.CourseId == CourseId).OrderBy(p => p.Index).ToListAsync();
            return TestList;
        }

        public async Task<Test> GetTestById(int Id)
        {
            var TestObj = await _db.Test.SingleOrDefaultAsync(p => p.Id == Id);
            return TestObj;
        }

        public async Task<ICollection<Test>> GetTestByTitle(string Title)
        {
            var TestList = await _db.Test.Where(p => p.Title.Contains(Title.ToLower().ToString())).ToListAsync();
            return TestList;
        }

        public async Task<bool> UpdateTest(Test Test)
        {
            var Datetime = _db.Test.AsNoTracking().FirstOrDefault(p => p.Id == Test.Id).CreatedDateTime;
            Test.CreatedDateTime = Datetime;
            _db.Test.Update(Test);
            return await Save();
        }

        public async Task<bool> UpdateTestStatus(int TestId, string status)
        {
            var TestObj = _db.Test.SingleOrDefault(p => p.Id == TestId);
            TestObj.Status = status;
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
