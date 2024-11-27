namespace WebApi
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Post[]> GetPostsAsync()
        {
            var client = _httpClientFactory.CreateClient("ExampleApi");
            var res = await client.GetFromJsonAsync<Post[]>("posts");

            return res;
        }
    }
}
