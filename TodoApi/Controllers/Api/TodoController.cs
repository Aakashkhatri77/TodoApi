using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models;

namespace TodoApi.Controllers.Api
{


    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodoController(TodoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {

            var todo = await _context.Todo.ToListAsync();
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> PostTodos(Todo todo)
        {
            if (ModelState.IsValid == true)
            {
                var _todo = new Todo()
                {
                    Name = todo.Name,
                    Description = todo.Description,
                };
                _context.Todo.Add(_todo);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodos(int id, Todo todo)
        {
            if (ModelState.IsValid == true)
            {
                var _todo = await _context.Todo.FindAsync(id);
                _todo.Name = todo.Name;
                _todo.Description = todo.Description;
                _context.Todo.Update(_todo);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodos(int id)
        {
            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}