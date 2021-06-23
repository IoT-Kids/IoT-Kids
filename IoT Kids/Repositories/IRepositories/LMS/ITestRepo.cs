using IoT_Kids.Models.LMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.LMS
{
    public interface ITestRepo
    {
        Task<bool> CreateTest(Test Test);
        Task<ICollection<Test>> GetAllTests();
        Task<ICollection<Test>> GetCourseTests(int CourseId);
        Task<ICollection<Test>> GetTestByTitle(string Title);
        Task<Test> GetTestById(int Id);
        Task<bool> UpdateTest(Test Test);
        Task<bool> UpdateTestStatus(int TestId, string status);
        Task<bool> DeleteTest(int TestId);
    }
}
