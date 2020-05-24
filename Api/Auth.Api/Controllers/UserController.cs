using Auth.Domain.Services.User;
using Auth.Domain.Services.User.Dto;
using Auth.SharedKernel.Domain.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotification _notification;

        public UserController(IUserService userService, INotification notification)
        {
            _userService = userService;
            _notification = notification;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok("Ping..");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(UserDto user)
        {
            if(!_userService.Post(user))
                return BadRequest(_notification.GetErrors());

            return Ok(new { user.Name, user.Email });
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Authenticate(UserDto user)
        {
            var userData = _userService.Get(user.Email, user.PasswordHash);

            if (_notification.Any())
                return BadRequest(_notification.GetErrors());

            var token = _userService.GenerateToken(user);
            userData.PasswordHash = string.Empty;

            return Ok(new { userData, token });
        }

    }
}