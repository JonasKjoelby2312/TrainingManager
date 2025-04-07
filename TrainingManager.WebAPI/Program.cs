
using System.Runtime.CompilerServices;
using TrainingManager.DataAccess.DAOs;

namespace TrainingManager.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        //SSMS connection string:
        const string connectionString = "Server=tcp:hildur.ucn.dk,1433;Database=DMA-CSD-S232_10503097;User ID=DMA-CSD-S232_10503097;Password=Password1!;";

        builder.Services.AddSingleton<IEmployeeDAO>((_) => (IEmployeeDAO)new EmployeeDAO(connectionString));


        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
