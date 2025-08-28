using LibraryApp.Api.Middleware;
using LibraryApp.Api.Services.Implementations;
using LibraryApp.Api.Services.Interfaces;
using LibraryApp.Infrastructure;


var builder = WebApplication.CreateBuilder(args);


// Controllers + API Versioning
builder.Services.AddControllers();
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
});


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// CORS para Angular
const string CorsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, p =>
    p.WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());
});


// Infrastructure (DbContext MySQL)
builder.Services.AddInfrastructure(builder.Configuration);


// Services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IBookService, BookService>();


var app = builder.Build();


app.UseMiddleware<ErrorHandlingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.MapControllers();
app.Run();