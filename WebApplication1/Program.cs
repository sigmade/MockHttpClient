using WebApi.Middleware;

namespace WebApi
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

            builder.Services.AddHttpClient("ExampleApi", client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddTransient<ArticleService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
