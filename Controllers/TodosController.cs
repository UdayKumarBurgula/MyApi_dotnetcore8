using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Dtos;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _db;
    public TodosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<TodoReadDto>>> GetAll()
    {
        var items = await _db.TodoItems.AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => new TodoReadDto
            {
                Id = x.Id,
                Title = x.Title,
                IsDone = x.IsDone,
                CreatedUtc = x.CreatedUtc
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoReadDto>> GetById(int id)
    {
        var item = await _db.TodoItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound();

        return Ok(new TodoReadDto
        {
            Id = item.Id,
            Title = item.Title,
            IsDone = item.IsDone,
            CreatedUtc = item.CreatedUtc
        });
    }

    [HttpPost]
    public async Task<ActionResult<TodoReadDto>> Create([FromBody] TodoCreateDto dto)
    {
        var entity = new TodoItem { Title = dto.Title.Trim(), IsDone = dto.IsDone };
        _db.TodoItems.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new TodoReadDto
        {
            Id = entity.Id,
            Title = entity.Title,
            IsDone = entity.IsDone,
            CreatedUtc = entity.CreatedUtc
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoUpdateDto dto)
    {
        var entity = await _db.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        entity.Title = dto.Title.Trim();
        entity.IsDone = dto.IsDone;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        _db.TodoItems.Remove(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
