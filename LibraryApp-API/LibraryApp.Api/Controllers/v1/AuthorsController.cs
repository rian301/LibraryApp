using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/authors")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _service;
    public AuthorsController(IAuthorService service) => _service = service;


    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<AuthorDto>>>> GetAll() =>
    ApiResponse<List<AuthorDto>>.Ok(await _service.GetAllAsync());


    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<AuthorDto?>>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item is null) return NotFound(ApiResponse<AuthorDto?>.Fail("Autor não encontrado."));
        return ApiResponse<AuthorDto?>.Ok(item);
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse<AuthorDto>>> Create([FromBody] AuthorCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id, version = "1" }, ApiResponse<AuthorDto>.Ok(created));
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] AuthorUpdateDto dto)
    {
        var ok = await _service.UpdateAsync(id, dto);
        if (!ok) return NotFound(ApiResponse<object>.Fail("Autor não encontrado."));
        return ApiResponse<object>.Ok(new { Id = id });
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound(ApiResponse<object>.Fail("Autor não encontrado."));
        return ApiResponse<object>.Ok(new { Id = id });
    }
}