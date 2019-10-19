using AutoMapper;
using ComputerScienceBlogBackEnd.DataAccess;

namespace ComputerScienceBlogBackEnd.Services.UserManagement
{
    public class CustomerManagementMappingProfile : Profile
    {
        public CustomerManagementMappingProfile()
        {
            CreateMap<User, UserModel>();
        }
    }
}
