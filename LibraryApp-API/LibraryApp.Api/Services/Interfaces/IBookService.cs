using LibraryApp.Api.Models;

namespace LibraryApp.Api.Services.Interfaces;

public interface IBookService
{
    Task<List<BookViewModel>> GetAllAsync();
    Task<BookViewModel?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(BookCreateDto dto);
    Task<bool> UpdateAsync(int id, BookUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}