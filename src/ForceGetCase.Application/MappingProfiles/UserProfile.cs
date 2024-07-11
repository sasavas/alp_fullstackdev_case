using AutoMapper;
using ForceGetCase.Application.Models.User;
using ForceGetCase.DataAccess.Identity;

namespace ForceGetCase.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
    }
}
