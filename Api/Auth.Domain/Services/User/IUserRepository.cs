using System.Collections.Generic;
using Auth.Domain.Services.User.Entities;

namespace Auth.Domain.Services.User
{
    public interface IUserRepository
    {
         void Post(UserEntity user);
         IEnumerable<UserEntity> Get();
         UserEntity GetByEmail(string email);
         UserEntity GetByEmailAndPassword(string email, string password);
    }
}