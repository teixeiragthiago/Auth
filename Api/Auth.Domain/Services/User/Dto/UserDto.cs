using System;
using Auth.SharedKernel;
using Auth.SharedKernel.Domain.Notification;
using MongoDB.Bson;

namespace Auth.Domain.Services.User.Dto
{
    public class UserDto
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Gender { get; set; }
        public DateTime Birth { get; set; }

        public bool IsValidInsert(INotification _notification)
        {
            if(!Email.IsValidEMail())
                return _notification.AddWithReturn<bool>("O E-mail informado não é válido!");

            if(string.IsNullOrEmpty(Name))
                return _notification.AddWithReturn<bool>("O Nome informado não é válido!");

            if(string.IsNullOrEmpty(PasswordHash))
                return _notification.AddWithReturn<bool>("A Senha informada não é válido!");

            if(string.IsNullOrEmpty(Gender))
                return _notification.AddWithReturn<bool>("O Gênero informado não é válido!");

            if(Birth == null)
                return _notification.AddWithReturn<bool>("A Data de nasciment informada não é válido!");
            
            return true;
        }

    }
}