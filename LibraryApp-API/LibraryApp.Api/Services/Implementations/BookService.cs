using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Interfaces;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Services.Implementations;

public class BookService : IBookService
{
    private readonly LibraryDbContext _db;
    public BookService(LibraryDbContext db) => _db = db;

    public async Task<List<BookViewModel>> GetAllAsync() =>
    await _db.Books.AsNoTracking()
    .Include(b => b.Author)
    .Include(b => b.Genre)
    .OrderBy(b => b.Title)
    .Select(b => new BookViewModel(
    b.Id,
    b.Title,
    b.AuthorId,
    b.Author!.Name,
    b.GenreId,
    b.Genre!.Name
    ))
    .ToListAsync();

    public async Task<BookViewModel?> GetByIdAsync(int id) =>
    await _db.Books.AsNoTracking()
    .Include(b => b.Author)
    .Include(b => b.Genre)
    .Where(b => b.Id == id)
    .Select(b => new BookViewModel(
    b.Id, b.Title, b.AuthorId, b.Author!.Name, b.GenreId, b.Genre!.Name))
    .FirstOrDefaultAsync();

    public async Task<BookDto> CreateAsync(BookCreateDto dto)
    {
        if (!await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId))
            throw new InvalidOperationException("Autor inexistente.");
        if (!await _db.Genres.AnyAsync(g => g.Id == dto.GenreId))
            throw new InvalidOperationException("Gênero inexistente.");

        var entity = new Book { Title = dto.Title, AuthorId = dto.AuthorId, GenreId = dto.GenreId };
        _db.Books.Add(entity);
        await _db.SaveChangesAsync();
        return new BookDto(entity.Id, entity.Title, entity.AuthorId, entity.GenreId);
    }

    public async Task<bool> UpdateAsync(int id, BookUpdateDto dto)
    {
        var entity = await _db.Books.FindAsync(id);
        if (entity is null) return false;

        if (!await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId))
            throw new InvalidOperationException("Autor inexistente.");
        if (!await _db.Genres.AnyAsync(g => g.Id == dto.GenreId))
            throw new InvalidOperationException("Gênero inexistente.");

        entity.Title = dto.Title;
        entity.AuthorId = dto.AuthorId;
        entity.GenreId = dto.GenreId;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Books.FindAsync(id);
        if (entity is null) return false;
        _db.Books.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}