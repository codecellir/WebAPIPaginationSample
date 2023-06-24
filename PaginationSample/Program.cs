using Bogus;
using PaginationSample.Data;
using PaginationSample.Entities;
using PaginationSample.Models;
using PaginationSample.Repsitories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();


//app.MapGet("/student", async (IStudentRepository repository) =>
//{
//    return Results.Ok(await repository.GetAllAsync());
//});

app.MapGet("/student", async (string? searchTerm, string? sortColumn, string? sortOrder, int page, int pageSize, IStudentRepository repository) =>
{
    var request = new GetStudentQuery(page, pageSize, searchTerm, sortColumn, sortOrder);
    return Results.Ok(await repository.GetAllAsync(request));
});

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<AppDbContext>();
await context.Students.AddRangeAsync(GenerateFakeData());
await context.SaveChangesAsync();


app.Run();

IEnumerable<Student> GenerateFakeData()
{
    for (int i = 0; i < 250; i++)
    {
        yield return new Faker<Student>()
            .RuleFor(d => d.Id, f => f.Random.Int(1000))
            .RuleFor(d => d.Name, f => f.Name.FullName())
            .RuleFor(d => d.DebtorAmount, f => f.Random.Decimal(1000, 1_000_000_000))
            .RuleFor(d => d.Gender, f => f.Person.Gender.ToString())
            .Generate();
    }
}