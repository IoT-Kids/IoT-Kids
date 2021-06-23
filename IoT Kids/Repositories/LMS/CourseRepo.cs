using IoT_Kids.Data;
using IoT_Kids.Models.LMS;
using IoT_Kids.Repositories.IRepositories.LMS;
using IoT_Kids.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.Repositories.LMS
{
    public class CourseRepo : ICourseRepo
    {
        private readonly ApplicationDbContext _db;

        public CourseRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateCourse(Course Course)
        {
            _db.Course.Add(Course);
            return await Save();
        }

        public async Task<bool> DeleteCourse(int CourseId)
        {
            var CourseObj = await _db.Course.SingleOrDefaultAsync(p => p.Id == CourseId);
            if (CourseObj == null)
            {
                return false;
            }
            _db.Course.Remove(CourseObj);
            return await Save();
        }

        public async Task<Course> GetCourseById(int Id)
        {
            var CourseObj = await _db.Course.SingleOrDefaultAsync(p => p.Id == Id);

            return CourseObj;
        }

        public async Task<ICollection<Course>> GetCourseByTitle(string Title)
        {
            var CourseList = await _db.Course.Where(p => p.Title.Contains(Title.ToLower().ToString())).ToListAsync();

            return CourseList;
        }

        public async Task<bool> UpdateCourse(Course Course)
        {
            var Datetime = _db.Course.AsNoTracking().FirstOrDefault(p => p.Id == Course.Id).CreatedDateTime;
            Course.CreatedDateTime = Datetime;
            _db.Course.Update(Course);
            return await Save();
        }

        public async Task<bool> UpdateCourseStatus(int CourseId, string status)
        {
            var CourseObj = _db.Course.SingleOrDefault(p => p.Id == CourseId);
            CourseObj.Status = status;
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<ICollection<Course>> GetAllCourses()
        {
            var CourseList = await _db.Course.OrderBy(p => p.Index).ToListAsync();
            return CourseList;
        }

        public Task<ICollection<Course>> GetAllCoursesWithLessons()
        {
            throw new NotImplementedException();
        }

    ////    public async Task<ICollection<CourseLessonVM>> GetCourseWithLessonsById(int Id)
    //    {
    //        var CourseObj = await (from co in _db.Course
    //                               join le in _db.Lesson on co.Id equals le.CourseID
    //                               where co.Id.Equals(Id)
    //                               select new CourseLessonVM
    //                               {
    //                                   CourseId = co.Id,
    //                                   CourseTitle = co.Title,
    //                                   CourseDescription = co.Description, 
    //                                   Image = co.Image,
    //                                   Lang = co.Lang,
    //                                   Status = co.Status,
    //                                   Index = co.Index, 
    //                                   CreatedDateTime = co.CreatedDateTime.ToString("dd/MM/yyyy"),
    //                                   LessonId = le.Id, 
    //                                   LessonDescription = le.Description,
    //                                   LessonIndex = le.Index, 
    //                                   Content = le.Content,
    //                                   VideoURL = le.VideoURL, 
    //                                   SampleVideo = le.SampleVideo,
    //                                   HasTest = le.HasTest,

    //                               }).ToListAsync();
    //                            return CourseObj;
    //    }
    }
}
