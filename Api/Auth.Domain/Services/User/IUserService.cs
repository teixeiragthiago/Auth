using System.Collections.Generic;
using Auth.Domain.Services.User.Dto;

namespace Auth.Domain.Services.User
{
    public interface IUserService
    {
         bool Post(UserDto user);
         string GenerateToken(UserDto user);
        UserDto GetByEmailAndPassword(string email, string password);
        IEnumerable<UserDto> Get(out int total, int? page = null, int? paginateQuantity = null, string email = null, string name = null, string gender = null);
    }
}