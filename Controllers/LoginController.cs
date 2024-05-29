 using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tasks.Services;
using User.Interfaces;

namespace User.Controllers
{
    using User.Models;

    [ApiController]
    [Route("[action]")]
    public class LoginController : ControllerBase
    {
        IUserService userService;

        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] User user)
        {
            var claims = new List<Claim>();
            var getUser = userService.GetAll()?.FirstOrDefault(c => c.Name == user.Name && c.Password == user.Password);
            if (getUser == null)
                return Unauthorized();
            if (getUser.IsAdmin)
            {
                claims.Add(
                    new Claim("type", "Admin")
                );
            }

            claims.Add(
                new Claim("type", "User")
            );

            claims.Add(new Claim("Id", getUser.Id.ToString()));
            return new OkObjectResult(TokenService.WriteToken(TokenService.GetToken(claims)));
        }

    }

}