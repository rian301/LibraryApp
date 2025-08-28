using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Interfaces;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Services.Implementations;
public class GenreService : IGenreService
{
    private readonly LibraryDbContext _db;
    public GenreService(LibraryDbContext db) => _db = db;

    public async Task<List<GenreDto>> GetAllAsync() =>
    await _db.Genres.AsNoTracking()
    .OrderBy(g => g.Name)
    .Select(g => new GenreDto(g.Id, g.Name))
    .ToListAsync();

    public async Task<GenreDto?> GetByIdAsync(int id) =>
    await _db.Genres.AsNoTracking()
    .Where(g => g.Id == id)
    .Select(g => new GenreDto(g.Id, g.Name))
    .FirstOrDefaultAsync();

    public async Task<GenreDto> CreateAsync(GenreCreateDto dto)
    {
        var entity = new Genre { Name = dto.Name };
        _db.Genres.Add(entity);
        await _db.SaveChangesAsync();
        return new GenreDto(entity.Id, entity.Name);
    }

    public async Task<bool> UpdateAsync(int id, GenreUpdateDto dto)
    {
        var entity = await _db.Genres.FindAsync(id);
        if (entity is null) return false;
        entity.Name = dto.Name;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hasBooks = await _db.Books.AnyAsync(b => b.GenreId == id);
        if (hasBooks) throw new InvalidOperationException("Gênero possui livros associados.");

        var entity = await _db.Genres.FindAsync(id);
        if (entity is null) return false;
        _db.Genres.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}