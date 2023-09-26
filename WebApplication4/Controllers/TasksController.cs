using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DataDB;
using WebApplication4.Filters;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    //[ApiKeyAuth]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TestApiContext _context;

        public TasksController(TestApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Tasks
        /// </summary>
        /// <remarks>Get Tasks List</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }

          var tasks = await _context.Tasks.Include(u => u.AssigneeNavigation).ToListAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Get a Task
        /// </summary>
        /// <remarks>Get a Task by Id</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        /// <summary>
        /// Edit a Task
        /// </summary>
        /// <remarks>Edit a Task</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="204">Successfully Edited</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Models.Task task)
        {
            if (id != task.ID)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        /// <summary>
        /// Add a Task
        /// </summary>
        /// <remarks>Add a Task</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="201">Successfully Created</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
        {
          if (_context.Tasks == null)
          {
              return Problem("Entity set 'TestApiContext.Tasks'  is null.");
          }
            _context.Tasks.Add(task);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TaskExists(task.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTask", new { id = task.ID }, task);
        }

        /// <summary>
        /// Delete a Task
        /// </summary>
        /// <remarks>Delete a Task by Id</remarks>
        /// <response code="200">Successfull</response>
        /// <response code="204">Successfully Deleted</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="500">Sorry, Error on our side</response>
        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
