namespace LibraryApp.Api.Models;

public record AuthorDto(int Id, string Name);
public record AuthorCreateDto(string Name);
public record AuthorUpdateDto(string Name);