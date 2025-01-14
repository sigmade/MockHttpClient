using System.Text.Json;

namespace WebApi
{
    public class ArticleService(IHttpClientFactory httpClientFactory)
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<Article?> GetArticleById()
        {
            var client = _httpClientFactory.CreateClient("ArticleApi");

            using var response = await client.GetAsync("article");

            if (!response.IsSuccessStatusCode)
            {
                // Optionally log or handle the error here
                return null;
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var article = JsonSerializer.Deserialize<Article>(responseJson);

            return article;
        }
    }
}
