using IoT_Kids.Models.LMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.LMS
{
    public interface ICourseRepo
    {
        Task<bool> CreateCourse(Course Course);
        Task<ICollection<Course>> GetAllCourses();
        Task<Course> GetCourseByTitle(string Title);
        Task<Course> GetCourseById(int Id);

        Task<bool> UpdateCourse(Course Course);
        Task<bool> UpdateCourseStatus(int CourseId, string status);
        Task<bool> DeleteCourse(int CourseId);
    }
}
