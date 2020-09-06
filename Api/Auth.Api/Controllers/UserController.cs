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

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping..");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get(int? page = null, int? paginateQuantity = null, string email = null, string name = null, string gender = null)
        {
            var users = _userService.Get(out int total, page, paginateQuantity, email, name, gender);

            if (_notification.Any())
                return BadRequest(_notification.GetErrors());

            return Ok(new { total, users });
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
            var userData = _userService.GetByEmailAndPassword(user.Email, user.PasswordHash);

            if (_notification.Any())
                return BadRequest(_notification.GetErrors());

            var token = _userService.GenerateToken(user);
            userData.PasswordHash = string.Empty;

            return Ok(new { userData, token });
        }

    }
}