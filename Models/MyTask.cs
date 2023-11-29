using System;

namespace Tasks.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDone { get; set; }
        public int UserId { get; set; }
    }
}