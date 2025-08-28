using LibraryApp.Api.Models;
using LibraryApp.Api.Services.Implementations;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Tests;

public class GenreServiceTests
{
    private static LibraryDbContext InMemory()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
        return new LibraryDbContext(options);
    }

    [Fact]
    public async Task Should_Create_Genre()
    {
        using var db = InMemory();
        var svc = new GenreService(db);
        var created = await svc.CreateAsync(new GenreCreateDto("Fantasia"));
        Assert.True(created.Id > 0);
        Assert.Equal("Fantasia", created.Name);
    }

    [Fact]
    public async Task Should_Update_Genre_Name()
    {
        using var db = InMemory();
        // Arrange
        var genre = new Genre { Name = "Aventura" };
        db.Genres.Add(genre);
        await db.SaveChangesAsync();

        var svc = new GenreService(db);

        // Act
        var result = await svc.UpdateAsync(genre.Id, new GenreUpdateDto("Aventura e Ação"));

        // Assert
        Assert.True(result);

        var fromDb = await db.Genres.AsNoTracking().SingleAsync(g => g.Id == genre.Id);
        Assert.Equal("Aventura e Ação", fromDb.Name);
    }

    [Fact]
    public async Task Should_Return_False_When_Updating_Nonexistent_Genre()
    {
        using var db = InMemory();
        var svc = new GenreService(db);

        var result = await svc.UpdateAsync(9999, new GenreUpdateDto("Qualquer"));

        Assert.False(result);
    }

    [Fact]
    public async Task Should_Block_Delete_When_Has_Books()
    {
        using var db = InMemory();
        var genre = new Genre { Name = "Romance" };
        var author = new Author { Name = "Autor" };
        db.Genres.Add(genre);
        db.Authors.Add(author);
        await db.SaveChangesAsync();
        db.Books.Add(new Book { Title = "Livro", AuthorId = author.Id, GenreId = genre.Id });
        await db.SaveChangesAsync();


        var svc = new GenreService(db);
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => svc.DeleteAsync(genre.Id));
        Assert.Contains("associados", ex.Message);
    }
}