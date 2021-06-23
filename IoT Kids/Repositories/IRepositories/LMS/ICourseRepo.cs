using IoT_Kids.Models.LMS;
using IoT_Kids.ViewModels;
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
      //  Task<ICollection<Course>> GetAllCoursesWithLessons();
        Task<ICollection<Course>> GetCourseByTitle(string Title);
        Task<Course> GetCourseById(int Id);
      //  Task<ICollection<CourseLessonVM>> GetCourseWithLessonsById(int Id);

        Task<bool> UpdateCourse(Course Course);
        Task<bool> UpdateCourseStatus(int CourseId, string status);
        Task<bool> DeleteCourse(int CourseId);
    }
}
