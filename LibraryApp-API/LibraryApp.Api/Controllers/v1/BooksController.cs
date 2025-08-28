using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Api.Controllers.v1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    public BooksController(IBookService service) => _service = service;


    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<BookViewModel>>>> GetAll() =>
    ApiResponse<List<BookViewModel>>.Ok(await _service.GetAllAsync());


    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<BookViewModel?>>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound(ApiResponse<BookViewModel?>.Fail("Livro não encontrado."));
        return ApiResponse<BookViewModel?>.Ok(item);
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookDto>>> Create([FromBody] BookCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id, version = "1" }, ApiResponse<BookDto>.Ok(created));
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] BookUpdateDto dto)
    {
        var ok = await _service.UpdateAsync(id, dto);
        if (!ok) return NotFound(ApiResponse<object>.Fail("Livro não encontrado."));
        return ApiResponse<object>.Ok(new { Id = id });
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound(ApiResponse<object>.Fail("Livro não encontrado."));
        return ApiResponse<object>.Ok(new { Id = id });
    }
}