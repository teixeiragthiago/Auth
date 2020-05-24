using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Domain.Services.User.Dto;
using Auth.Domain.Services.User.Entities;
using Auth.SharedKernel;
using Auth.SharedKernel.Domain.Notification;
using Auth.SharedKernel.Secret;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Domain.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotification _notification;
        private readonly IMapper _mapper;

        public UserService(INotification notification, IUserRepository userRepository, IMapper mapper)
        {
            _notification = notification;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public string GenerateToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Secret.key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials =
                    new SigningCredentials 
                    (
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256
                    )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<UserDto> Get(out int total, int? page = null, int? paginateQuantity = null, string email = null, string name = null, string gender = null)
        {
            return _mapper.Map<IEnumerable<UserDto>>(_userRepository.Get(out total, page, paginateQuantity, email, name, gender));
        }

        public UserDto GetByEmailAndPassword(string email, string password)
        {
            var user = _userRepository.GetByEmailAndPassword(email, Encryption.Encrypt(password.ToLower()));

            if (user == null)
                return _notification.AddWithReturn<UserDto>("O E-mail ou a Senha estão incorretos!");

            return new UserDto
            {
                Email = user.Email,
                Name = user.Name,
                Gender = user.Gender,
                Birth = user.Birth
            };
        }

        public bool Post(UserDto user)
        {
            if(!user.IsValidInsert(_notification))
                return false;

            var userData = _userRepository.GetByEmail(user.Email);
            if (userData != null)
                return _notification.AddWithReturn<bool>("O E-mail informado já possui cadastro!");

            _userRepository.Post(new UserEntity
            {
                Name = user.Name,
                Email = user.Email,
                PasswordHash = Encryption.Encrypt(user.PasswordHash.ToLower()),
                Gender = user.Gender,
                Birth = user.Birth
            });

            return true;
        }
    }
}