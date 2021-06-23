using IoT_Kids.Models.LMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.IRepositories.LMS
{
    public interface ILessonRepo
    {
        Task<bool> CreateLesson(Lesson Lesson);
        Task<ICollection<Lesson>> GetAllLessons();
        Task<ICollection<Lesson>> GetCourseLessons(int CourseId);
        Task<ICollection<Lesson>> GetLessonByTitle(string Title);
        Task<Lesson> GetLessonById(int Id);
        Task<bool> UpdateLesson(Lesson Lesson);
        Task<bool> UpdateLessonStatus(int LessonId, string status);
        Task<bool> DeleteLesson(int LessonId);
      
    }
}
