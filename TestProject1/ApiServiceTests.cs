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

            var expectedPosts = new[]
            {
                    new Post { Id = 1, Title = "Test Post 1", Body = "Test Body 1" },
                };

            var jsonResponse = JsonSerializer.Serialize(expectedPosts);
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };
            var mockHttpMessageHandler = new MockHttpMessageHandler(httpResponse);
            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://example.com/")
            };

            var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
            mockHttpClientFactory.CreateClient("ExampleApi").Returns(httpClient);

            var apiService = new ApiService(mockHttpClientFactory);

            // Act
            var result = await apiService.GetPostsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPosts[0].Id, result[0].Id);
            Assert.Equal(expectedPosts[0].Title, result[0].Title);
        }

        [Fact]
        public async Task GetPostById_ReturnsPost()
        {
            // Arrange
            var expectedPost = new Post { Id = 1, Title = "Test Post 1", Body = "Test Body 1" };
            var jsonResponse = JsonSerializer.Serialize(expectedPost);
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };
            var mockHttpMessageHandler = new MockHttpMessageHandler(httpResponse);
            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://example.com/")
            };

            var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
            mockHttpClientFactory.CreateClient("ExampleApi").Returns(httpClient);

            var apiService = new ApiService(mockHttpClientFactory);

            // Act
            var result = await apiService.GetPostById();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPost.Id, result.Id);
            Assert.Equal(expectedPost.Title, result.Title);
        }
    }
}