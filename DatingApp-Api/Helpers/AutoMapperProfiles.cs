using System;
using System.Linq;
using AutoMapper;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;
using DatingApp_Api.Extension;

namespace DatingApp_Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(des => des.PhotoUrl, opti => opti.MapFrom(src =>
              src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(des => des.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<RegisterDto, AppUser>();

            CreateMap<Message, MessageDto>()
            .ForMember(des => des.SenderPhotoUrl, opti => opti.MapFrom(src =>
                  src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
               .ForMember(des => des.RecipientPhotoUrl, opti => opti.MapFrom(src =>
                  src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }


}
