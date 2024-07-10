
using Microsoft.EntityFrameworkCore;
using SimpleChat.Data;
using SimpleChat.Hubs;
using SimpleChat.Models;
using SimpleChat.Models.Abstractions.Repositories;
using SimpleChat.Models.Abstractions.Services;
using SimpleChat.Repositories;
using SimpleChat.Services;

namespace SimpleChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ChatAppContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IRepository<User>, Repository<User>>();
            builder.Services.AddScoped<IRepository<Chat>, Repository<Chat>>();
            builder.Services.AddScoped<IRepository<Message>, Repository<Message>>();

            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp v1"));
            }
           // app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");

            app.Run();

        }
    }
}
