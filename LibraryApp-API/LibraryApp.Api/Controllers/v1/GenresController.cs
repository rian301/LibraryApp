using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LibraryApp.Api.Controllers.v1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _service;
    public GenresController(IGenreService service) => _service = service;


    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<GenreDto>>>> GetAll() =>
    ApiResponse<List<GenreDto>>.Ok(await _service.GetAllAsync());


    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<GenreDto?>>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound(ApiResponse<GenreDto?>.Fail("Gênero não encontrado."));
        return ApiResponse<GenreDto?>.Ok(item);
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse<GenreDto>>> Create([FromBody] GenreCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id, version = "1" }, ApiResponse<GenreDto>.Ok(created));
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] GenreUpdateDto dto)
    {
        var ok = await _service.UpdateAsync(id, dto);
        if (!ok) return NotFound(ApiResponse<object>.Fail("Gênero não encontrado."));
        return ApiResponse<object>.Ok(new { Id = id });
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound(ApiResponse<object>.Fail("Gênero não encontrado."));
        return ApiResponse<object>.Ok(new { Id = id });
    }
}