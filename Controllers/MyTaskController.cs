using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tasks.Interfaces;

namespace Tasks.Controller
{
    using Tasks.Models;
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskService TaskService;

        private readonly int userId;

        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            this.userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value);

            this.TaskService = taskService;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<Tasks>> Get()
        {
            return TaskService.GetAll(userId);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<Tasks> Get(int id)
        {
            var task = TaskService.Get(userId, id);
            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        [Authorize(Policy = ("User"))]
        public ActionResult Post(Tasks t)
        {
            TaskService.Post(userId, t);
            return CreatedAtAction(nameof(Post), new { Id = t.Id }, t);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]

        public ActionResult Put(int id, [FromBody] Tasks task)
        {
            if (id != task.Id)
                return BadRequest("id <> task.Id");
            var item = TaskService.Get(userId, task.Id);
            if (item is null)
                return NotFound();

            TaskService.Update(userId, task);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policy = "User")]
        public ActionResult Delete(int id)
        {
            var task = TaskService.Get(userId, id);
            if (task == null)
                return NotFound();
            TaskService.Delete(userId, id);
            return NoContent();
        }

    }
}