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

        public async Task<Course> GetCourseByTitle(string Title)
        {
            var CourseObj = await _db.Course.SingleOrDefaultAsync(p => p.Title == Title);

            return CourseObj;
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
            var CourseObj =  _db.Course.SingleOrDefault(p => p.Id == CourseId);
            CourseObj.Status = status;
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<ICollection<Course>> GetAllCourses()
        {
            var CourseList = await _db.Course.OrderBy(p => p.Order).ToListAsync();
            return CourseList;
        }
    }
}
