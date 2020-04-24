using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Models
{
    public class CreateTodo
    {
        public string TaskDescription { get; set; }
    }

    public class Todo : CreateTodo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public bool IsCompleted { get; set; }
    }

    public class UpdateTodo : CreateTodo
    {
        public bool IsCompleted { get; set; }
    }
}
