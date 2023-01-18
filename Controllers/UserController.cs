using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task.Models;
using Task.Services;
using User.Models;
using User.Interfaces;
namespace User.Controllers
{
    using User.Models;
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User user)
        {
            var claims = new List<Claim>();
            var getUser = userService.GetAll().FirstOrDefault(c => c.Name == user.Name && c.Password == user.Password);
            if (getUser == null)
                return Unauthorized();
            if (getUser.IsAdmin)
            {

                claims.Add(
                    new Claim("type", "Admin")
                );
            }
            else
            {

                claims.Add(
                    new Claim("type", "User")
                );

            }

            claims.Add(new Claim("Id", getUser.Id.ToString()));

            var token = TokenService.GetToken(claims);

            return new OkObjectResult(TokenService.WriteToken(token));
        }


        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>> GetAll() =>
            userService.GetAll();


        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult<User> Get(int id)
        {
            var user = userService.Get(id);
            if (user == null)
                return NotFound();
            return user;
        }
        
        [HttpPost("{user}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Post(User user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.Id }, user);
        }

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user == null)
                return NotFound();
            userService.Delete(id);
            return NoContent();
        }

    }

}