using System.Collections.Generic;
using System.Linq;
using Auth.Domain.Services.User;
using Auth.Domain.Services.User.Entities;
using Auth.Infra.Base;
using Auth.SharedKernel;

namespace Auth.Infra.Repositories
{
    public class UserRepository : DapperBaseRepository, IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserEntity> Get(out int total, int? page = null, int? paginateQuantity = null, string email = null, string name = null, string gender = null)
        {
            var users = _context.mongoDb.Get<UserEntity>("user").AsQueryable();

            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower().Trim();
                users = users.Where(x => x.Email.ToLower().Trim() == email);
            }

            if (!string.IsNullOrEmpty(name))
                users = users.Where(x => x.Name.ToLower().Contains((name.ToLower())));

            if (!string.IsNullOrEmpty(gender))
                users = users.Where(x => x.Gender == gender);

            return users.Paginate(page, out total);
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