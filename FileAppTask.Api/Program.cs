using FileAppTask.Api.Services;
using FileAppTask.Data;
using FileAppTask.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("FileAppConnectionString");
builder.Services.AddDbContext<FileContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileService, FileSerivce>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddCors();


var app = builder.Build();

app.UseCors(x => x.AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allow any origin
                  .AllowCredentials());



// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
