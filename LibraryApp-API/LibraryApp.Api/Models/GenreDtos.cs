namespace LibraryApp.Api.Models;

public record GenreDto(int Id, string Name);
public record GenreCreateDto(string Name);
public record GenreUpdateDto(string Name);