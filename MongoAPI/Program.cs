using Microsoft.Extensions.Options;
using MongoAPI.Data;
using MongoAPI.Repositories;

namespace MongoAPI
{
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
            builder.Services.AddMongoContext<AppDbContext>(o => o.SetDatabaseName(builder.Configuration.GetValue<string>("MongoConf:DatabaseName")!)
                .SetConnectionString(builder.Configuration.GetConnectionString("MongoConnection")!));

            builder.Services.AddSingleton<IMarcaRepository, MarcaRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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