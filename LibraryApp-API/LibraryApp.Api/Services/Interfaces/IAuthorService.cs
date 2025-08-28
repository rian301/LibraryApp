using LibraryApp.Api.Models;

namespace LibraryApp.Api.Services.Interfaces;

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<AuthorDto> CreateAsync(AuthorCreateDto dto);
    Task<bool> UpdateAsync(int id, AuthorUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}