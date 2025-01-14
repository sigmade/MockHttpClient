using NSubstitute;
using System.Net;
using System.Text.Json;
using WebApi;

namespace UnitTests
{
    public class ApiServiceTests
    {
        [Fact]
        public async Task GetArticleById_ReturnsArticle()
        {
            // Arrange
            var expectedArticle = new Article { Id = 1, Title = "Test Title", Body = "Test Body" };
            var jsonResponse = JsonSerializer.Serialize(expectedArticle);
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };
            var mockHttpMessageHandler = new MockHttpMessageHandler((r, ct) => Task.FromResult(httpResponse));
            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://example.com/")
            };

            var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
            mockHttpClientFactory.CreateClient("ArticleApi").Returns(httpClient);

            var articleService = new ArticleService(mockHttpClientFactory);

            // Act
            var result = await articleService.GetArticleById();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedArticle.Id, result.Id);
            Assert.Equal(expectedArticle.Title, result.Title);
        }
    }
}