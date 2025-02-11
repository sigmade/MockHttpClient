using System.Text.Json;
using System.Text.Json.Serialization;

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

            string json = """
            {
              "client": {
                "clientType": "Company",
                "companyName": "ООО Ромашка"
              }
            }
            """;

            var order = JsonSerializer.Deserialize<Order>(json);

            using var response2 = await client.GetAsync("");

            return article;
        }
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "clientType")]
    [JsonDerivedType(typeof(Company), "Company")]
    [JsonDerivedType(typeof(Person), "Person")]
    public class ClientBase
    {
        // Можете добавить сюда общие поля, если нужны
    }

    public class Company : ClientBase
    {
        public string CompanyName { get; set; }
    }

    public class Person : ClientBase
    {
        public string FullName { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        //public string ClientType { get; set; }
        public ClientBase Client { get; set; }
    }

}
