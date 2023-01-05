using Microsoft.EntityFrameworkCore;
using ShoppingWepApi.Data;
using ShoppingWepApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<OfficeDB>(options =>
                                    options
                                    .UseNpgsql(connectionString)
                                    .UseSnakeCaseNamingConvention());

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.MapPost("/employees/", async (Employee e, OfficeDB db) =>
{
    db.Employees.Add(e);
    await db.SaveChangesAsync();

    return Results.Created($"/employee/.{e.Id}", e);
});

app.MapGet("/employee/{id:int}", async (int id, OfficeDB db) =>
{
    return await db.Employees.FindAsync(id)
        is Employee e
        ? Results.Ok(e)
        : Results.NotFound();
});
app.MapGet("/employess", async (OfficeDB db) => await db.Employees.ToListAsync());

app.MapPut("/employees/{Id:int}", async (int id, Employee e, OfficeDB db) =>
{
    if (e.Id != id)
        return Results.BadRequest();

    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    employee.FirstName = e.FirstName;
    employee.LastName = e.LastName;
    employee.Branch = e.Branch;
    employee.Age = e.Age;

    await db.SaveChangesAsync();

    return Results.Ok(employee);
});



app.MapDelete("/employee/{id:int}", async (int id, OfficeDB db) =>
{
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    db.Employees.Remove(employee);
    await db.SaveChangesAsync();

    return Results.NoContent();

});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
