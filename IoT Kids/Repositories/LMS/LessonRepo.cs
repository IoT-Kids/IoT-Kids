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
    public class LessonRepo : ILessonRepo
    {
        private readonly ApplicationDbContext _db;

        public LessonRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateLesson(Lesson Lesson)
        {
            _db.Lesson.Add(Lesson);
            return await Save();
        }

        public async Task<bool> DeleteLesson(int LessonId)
        {
            var LessonObj = await _db.Lesson.SingleOrDefaultAsync(p => p.Id == LessonId);
            if (LessonObj == null)
            {
                return false;
            }
            _db.Lesson.Remove(LessonObj);
            return await Save();
        }

        public async Task<ICollection<Lesson>> GetAllLessons()
        {
            var LessonList = await _db.Lesson.OrderBy(p => p.CreatedDateTime).ToListAsync();
            return LessonList;
        }

        public async Task<ICollection<Lesson>> GetCourseLessons(int CourseId)
        {
            var LessonList = await _db.Lesson.Where(p=> p.CourseID == CourseId).OrderBy(p => p.Index).ToListAsync();
            return LessonList;
        }

        public async Task<Lesson> GetLessonById(int Id)
        {
            var LessonObj = await _db.Lesson.SingleOrDefaultAsync(p => p.Id == Id);
            return LessonObj;
        }

        public async Task<ICollection<Lesson>> GetLessonByTitle(string Title)
        {
            var LessonList = await _db.Lesson.Where(p => p.Title.Contains(Title.ToLower().ToString())).ToListAsync();
            return LessonList;
        }

        public async Task<bool> UpdateLesson(Lesson Lesson)
        {
            var Datetime = _db.Lesson.AsNoTracking().FirstOrDefault(p => p.Id == Lesson.Id).CreatedDateTime;
            Lesson.CreatedDateTime = Datetime;
            _db.Lesson.Update(Lesson);
            return await Save();
        }

        public async Task<bool> UpdateLessonStatus(int LessonId, string status)
        {
            var LessonObj = _db.Lesson.SingleOrDefault(p => p.Id == LessonId);
            LessonObj.Status = status;
            return await Save();
        }
        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
