using AutoMapper;
using Mozaic.PasswordManager.BL;
using Mozaic.PasswordManager.Entities;
using Mozaic.PasswordManager.Web.Models.ViewModels;

namespace Mozaic.PasswordManager.Web
{
    public class AutoMapperConfiguration:AutoMapper.Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<UserAccount, UserAccountViewModel>().ReverseMap();
            CreateMap<UserAccount, SystemUserManager>().ReverseMap();
            CreateMap<SystemUser, SystemUserViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, SystemUser>().ReverseMap();
            CreateMap<EditUserViewModel, SystemUser>().ReverseMap();
        }
    }
}
