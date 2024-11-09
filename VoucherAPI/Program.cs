using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VoucherTokenGenerator.Services;

namespace VoucherAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create and run the application
            var builder = WebApplication.CreateBuilder(args);

            // Register services (e.g., TokenService)
            builder.Services.AddSingleton<TokenService>();

            // Add controllers
            builder.Services.AddControllers();

            // Add Swagger for API documentation (optional)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Build and configure the app
            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Default middlewares
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers(); // This is where you map the controllers to routes

            // Run the application
            app.Run();
        }
    }
}
