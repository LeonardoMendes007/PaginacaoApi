using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginacaoApi.Data;
using PaginacaoApi.Models;
using System.Collections.Generic;

namespace PaginacaoApi.Controllers
{
    [ApiController]
    [Route("api/todo")]
    public class TodoController : ControllerBase
    {
        [HttpGet(template:"load")]
        public async Task<IActionResult> LoadAsync([FromServices] AppDbContext context)
        {
            for (int i = 0; i < 1500; i++)
            {
                var todo = new Todo()
                {
                    Id = i + 1,
                    Done = true,
                    CreatedAt = DateTime.Now,
                    Title = $"Tarefa {i}"
                };

                await context.Todos.AddRangeAsync(todo);
                await context.SaveChangesAsync();
            }

            return Ok("Load Successfully");

        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 25)
        {
            var total = await context.Todos.CountAsync();
            var todos = await context
                .Todos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(new
            {
                total,
                skip,
                take,
                data = todos
            });
        }
    }
}
