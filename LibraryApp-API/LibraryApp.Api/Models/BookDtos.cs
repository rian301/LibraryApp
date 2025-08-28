namespace LibraryApp.Api.Models;

public record BookDto(int Id, string Title, int AuthorId, int GenreId);
public record BookCreateDto(string Title, int AuthorId, int GenreId);
public record BookUpdateDto(string Title, int AuthorId, int GenreId);

public record BookViewModel(int Id, string Title, int AuthorId, string AuthorName, int GenreId, string GenreName);