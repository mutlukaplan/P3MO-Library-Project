
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using P3MO.Application;
using P3MO.Domain;
using P3MO.Repository;
using P3MO.Repository.Data;

namespace P3MO.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

      

            builder.Services.AddDbContext<P3MOContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "P3MO Library Management API",
                    Version = "v1",
                    Description = "P3MO API for Library Management System"
                });
            });
            //http://localhost:5078/swagger/index.html

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBookManager, BookManager>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                // Apply migrations automatically in development
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<P3MOContext>();
                    dbContext.Database.Migrate();
                }
            }

            app.UseCors("AllowAllOrigins");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
