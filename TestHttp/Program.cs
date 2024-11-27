namespace HttpClientFactoryExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Создаем хост приложения
            var host = WebHost.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddHttpClient("ExampleApi", client =>
                    {
                        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    });

                    services.AddTransient<ApiService>();
                })
                .Build();

            // Получаем сервис и выполняем запрос
            var apiService = host.Services.GetRequiredService<ApiService>();
            var posts = await apiService.GetPostsAsync();

            foreach (var post in posts)
            {
                Console.WriteLine($"Post {post.Id}: {post.Title}");
            }
        }
    }
}
