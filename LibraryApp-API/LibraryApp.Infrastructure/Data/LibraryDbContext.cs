using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace LibraryApp.Infrastructure.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }


    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Book> Books => Set<Book>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Author>(e =>
        {
            e.ToTable("Authors");
            e.Property(p => p.Name).HasMaxLength(120).IsRequired();
        });


        modelBuilder.Entity<Genre>(e =>
        {
            e.ToTable("Genres");
            e.Property(p => p.Name).HasMaxLength(120).IsRequired();
        });


        modelBuilder.Entity<Book>(e =>
        {
            e.ToTable("Books");
            e.Property(p => p.Title).HasMaxLength(200).IsRequired();
            e.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(b => b.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
        });
    }
}