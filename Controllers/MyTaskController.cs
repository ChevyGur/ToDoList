using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Tasks.Services;
using Tasks.Interfaces;

namespace Tasks.Controller
{
    using Tasks.Models;
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskService TaskService;
            public TaskController(ITaskService taskService)
        {
            this.TaskService = taskService;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<Tasks>> Get()
        {
            return TaskService.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<Tasks> Get(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        [Authorize(Policy = ("User"))]
        public ActionResult Post(Tasks t)
        {
            TaskService.Post(t);
            return CreatedAtAction(nameof(Post), new { Id = t.Id }, t);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]

        public ActionResult Put(int id, [FromBody] Tasks task)
        {
            if (id != task.Id)
                return BadRequest("id <> task.Id");
            var res = TaskService.Update(task);
            if (!res)
                return NotFound();
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policy = "User")]
        public ActionResult Delete(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
                return NotFound();
            TaskService.Delete(id);
            return NoContent();
        }

    }
}