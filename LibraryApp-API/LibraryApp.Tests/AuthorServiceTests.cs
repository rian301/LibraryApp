using LibraryApp.Api.Services.Implementations;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Api.Models;

namespace LibraryApp.Tests;

public class AuthorServiceTests
{
    private static LibraryDbContext InMemory()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
        return new LibraryDbContext(options);
    }

    [Fact]
    public async Task Should_Create_Author()
    {
        using var db = InMemory();
        var service = new AuthorService(db);
        var created = await service.CreateAsync(new AuthorCreateDto("Teste"));
        Assert.True(created.Id > 0);
        Assert.Equal("Teste", created.Name);
    }
}