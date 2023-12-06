using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks.Services;
using User.Interfaces;

namespace User.Controllers
{

    using User.Models;
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserService userService;
        private readonly int userId;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userId = int.Parse(httpContextAccessor?.HttpContext?.User.FindFirst("Id")?.Value);
            this.userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<User>?> GetAll() => userService.GetAll();

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<User> GetMyUser()
        {
            var user = userService.Get(userId);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost("{user}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Post([FromBody] User user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user == null)
                return NotFound();
            userService.Delete( id);
            return NoContent();
        }

    }

}