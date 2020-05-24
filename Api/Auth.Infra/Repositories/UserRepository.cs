using System.Collections.Generic;
using System.Linq;
using Auth.Domain.Services.User;
using Auth.Domain.Services.User.Entities;

namespace Auth.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserEntity> Get()
        {
            return _context.mongoDb.Get<UserEntity>("user");
        }

        public UserEntity GetByEmail(string email)
        {
            email = email.Trim();

            return _context.mongoDb.Get<UserEntity>("user").FirstOrDefault(x => x.Email == email);
        }

        public UserEntity GetByEmailAndPassword(string email, string password)
        {
            return _context.mongoDb.Get<UserEntity>("user").FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.PasswordHash.ToLower() == password.ToLower());
        }

        public void Post(UserEntity user)
        {
            _context.mongoDb.Insert("user", user);
        }
    }
}