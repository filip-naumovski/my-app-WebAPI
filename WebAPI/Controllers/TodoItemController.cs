using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoDBContext _context;

        public TodoItemController(TodoDBContext context)
        {
            _context = context;
        }

        // GET: api/TodoItem
        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItems()
        {
            return _context.TodoItems;
        }

        // GET: api/TodoItem/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        // PUT: api/TodoItem/batch
        [HttpPut]
        [Route("batch")]
        public async Task<IActionResult> PutTodoItems([FromBody] TodoItem[] todos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach(TodoItem item in todos)
            {
                _context.Entry(item).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(item.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            

            return NoContent();
        }


        // PUT: api/TodoItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem([FromRoute] int id, [FromBody] TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            todoItem.id = id;

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItem
        [HttpPost]
        public async Task<IActionResult> PostTodoItem([FromBody] TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.id }, todoItem);
        }

        // DELETE: api/TodoItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.id == id);
        }
    }
}