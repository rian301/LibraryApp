using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Interfaces;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Api.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly LibraryDbContext _db;
    public AuthorService(LibraryDbContext db) => _db = db;

    public async Task<List<AuthorDto>> GetAllAsync() =>
    await _db.Authors.AsNoTracking()
    .OrderBy(a => a.Name)
    .Select(a => new AuthorDto(a.Id, a.Name))
    .ToListAsync();

    public async Task<AuthorDto?> GetByIdAsync(int id) =>
    await _db.Authors.AsNoTracking()
    .Where(a => a.Id == id)
    .Select(a => new AuthorDto(a.Id, a.Name))
    .FirstOrDefaultAsync();

    public async Task<AuthorDto> CreateAsync(AuthorCreateDto dto)
    {
        var entity = new Author { Name = dto.Name };
        _db.Authors.Add(entity);
        await _db.SaveChangesAsync();
        return new AuthorDto(entity.Id, entity.Name);
    }

    public async Task<bool> UpdateAsync(int id, AuthorUpdateDto dto)
    {
        var entity = await _db.Authors.FindAsync(id);
        if (entity is null) return false;
        entity.Name = dto.Name;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hasBooks = await _db.Books.AnyAsync(b => b.AuthorId == id);
        if (hasBooks) throw new InvalidOperationException("Autor possui livros associados.");

        var entity = await _db.Authors.FindAsync(id);
        if (entity is null) return false;
        _db.Authors.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}