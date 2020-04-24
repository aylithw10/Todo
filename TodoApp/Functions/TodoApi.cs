using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TodoApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace TodoApp.Functions
{
    public static class TodoApi
    {
        private static List<Todo> items = new List<Todo>();

        [FunctionName("Frank")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating new todo");
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateTodo>(body);

            var todo = new Todo() { TaskDescription = input.TaskDescription };
            items.Add(todo);
            return new OkObjectResult(todo);
        }


        [FunctionName("GetTodos")]
        public static IActionResult GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting todos");           
            return new OkObjectResult(items);
        }

        [FunctionName("GetTodoById")]
        public static IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("Getting todo");

            var todo = items.FirstOrDefault(x => x.Id == id);

            if (todo == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(todo);
        }


        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")] HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("Updating todo");

            var todo = items.FirstOrDefault(x => x.Id == id);

            if (todo == null)
            {
                return new NotFoundResult();
            }

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<UpdateTodo>(body);

            todo.IsCompleted = updated.IsCompleted;
            return new OkObjectResult(todo);
        }
    }
}
