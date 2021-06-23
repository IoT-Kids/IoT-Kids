using AutoMapper;
using IoT_Kids.Models;
using IoT_Kids.Models.Users.Dtos;
using IoT_Kids.Models.Memberships;
using IoT_Kids.Models.Memberships.Dtos;
using IoT_Kids.Models.Payments;
using IoT_Kids.Models.Payments.Dtos;
using IoT_Kids.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoT_Kids.Models.LMS;
using IoT_Kids.Models.LMS.Dtos;

namespace IoT_Kids.ModelMappers
{
    public class ModelMapping : Profile
    {
       public ModelMapping()
        {
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<MembershipPlan, MembershipPlanDto>().ReverseMap();
            CreateMap<MembershipPlan, MembershipPlanCreateDto>().ReverseMap();
            CreateMap<Member, MemberDto>().ReverseMap();
            CreateMap<Member, MemberUpdateDto>().ReverseMap();
            CreateMap<Member, MemberCreateDto>().ReverseMap();
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Coupon, CouponCreateDto>().ReverseMap();
            CreateMap<Coupon, CouponUpdateDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Payment, PaymentCreateDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CrUpCourseDto>().ReverseMap();
            CreateMap<Lesson, LessonDto>().ReverseMap();
            CreateMap<Lesson, CrUpLessonDto>().ReverseMap();
            CreateMap<Test, CrUpTestDto>().ReverseMap();
            CreateMap<Test, TestDto>().ReverseMap();
            CreateMap<TestQuestion, TestQuestionDto>().ReverseMap();
            CreateMap<TestQuestion, CrUpTestQuestionDto>().ReverseMap();
            CreateMap<QuestionChoice, QuestionChoiceDto>().ReverseMap();
            CreateMap<QuestionChoice, CrUpQuestionChoiceDto>().ReverseMap();
            //  CreateMap<Payment, PaymentUpdateDto>().ReverseMap();
        }
    }
}
