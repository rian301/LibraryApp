using LibraryApp.Api.Models;

namespace LibraryApp.Api.Services.Interfaces;

public interface IGenreService
{
    Task<List<GenreDto>> GetAllAsync();
    Task<GenreDto?> GetByIdAsync(int id);
    Task<GenreDto> CreateAsync(GenreCreateDto dto);
    Task<bool> UpdateAsync(int id, GenreUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}