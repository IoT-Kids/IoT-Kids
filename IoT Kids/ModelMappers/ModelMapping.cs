using AutoMapper;
using IoT_Kids.Models;
using IoT_Kids.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
          //  CreateMap<Payment, PaymentUpdateDto>().ReverseMap();
        }
    }
}
