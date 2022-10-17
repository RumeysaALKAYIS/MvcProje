using AutoMapper;
using MvcProject.Entities;
using MvcProject.Models;

namespace MvcProject
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, CreateUserModel>().ReverseMap();
            CreateMap<User, EditUserModel>().ReverseMap();
        }
    }
}
