using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Task.Services;
using Task.Interfaces;

namespace Task.Controller
{
    using Task.Models;
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {

        ITaskService TaskService;

        public TaskController(ITaskService TaskService)
        {
            this.TaskService = TaskService;
        }



        [HttpGet("{token}")]
        public ActionResult<IEnumerable<Task>> Get(string token)
        {
            Console.WriteLine(token);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            return TaskService.GetAll(TokenService.decode(token));

        }

        // [HttpGet("{id}")]
        // public ActionResult<Task> Get(int id)
        // {
        //     var task = TaskService.Get(id);
        //     if (task == null)
        //         return NotFound();
        //     return task;
        // }

        [HttpPost]
        public ActionResult Post(Task t)
        {
            TaskService.Post(t);
            return CreatedAtAction(nameof(Post), new { Id = t.Id }, t);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest("id <> task.Id");
            var res = TaskService.Update(task);
            if (!res)
                return NotFound();
            return NoContent();
        }

        [HttpDelete]
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