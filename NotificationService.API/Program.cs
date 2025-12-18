
using NotificationService.API.Options;
using NotificationService.API.Services;
using NotificationService.Shared.Rabbit.Options;

namespace NotificationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddLogging();
            builder.Services.Configure<QueueOptions>(config.GetSection(nameof(QueueOptions)));
            builder.Services.Configure<RabbitMQOptions>(config.GetSection(nameof(RabbitMQOptions)));
			builder.Services.AddSingleton<AdvancedPublishClient>();
            builder.Services.AddScoped<NotificationService.API.Services.NotificationService>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
