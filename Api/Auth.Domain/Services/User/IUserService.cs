using Auth.Domain.Services.User.Dto;

namespace Auth.Domain.Services.User
{
    public interface IUserService
    {
         bool Post(UserDto user);
         string GenerateToken(UserDto user);
        UserDto Get(string email, string password);
    }
}