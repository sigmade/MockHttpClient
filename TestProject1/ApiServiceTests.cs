using NSubstitute;
using System.Net;
using System.Text.Json;
using WebApi;

namespace UnitTests
{
    public class ApiServiceTests
    {
        [Fact]
        public async Task GetPostsAsync_ReturnsPosts()
        {
            // Arrange
            var mockHttpMessageHandler = new TestMessageHandler();
            var expectedPosts = new[]
            {
                new Post { Id = 1, Title = "Test Post 1", Body = "Test Body 1" },
                new Post { Id = 2, Title = "Test Post 2", Body = "Test Body 2" }
            };

            var jsonResponse = JsonSerializer.Serialize(expectedPosts);
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };

            mockHttpMessageHandler.SetResponse(httpResponse);

            var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://example.com/")
            };

            mockHttpClientFactory.CreateClient("ExampleApi").Returns(httpClient);

            var apiService = new ApiService(mockHttpClientFactory);

            // Act
            var result = await apiService.GetPostsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPosts.Length, result.Length);
            Assert.Equal(expectedPosts[0].Id, result[0].Id);
            Assert.Equal(expectedPosts[0].Title, result[0].Title);
        }
    }
}