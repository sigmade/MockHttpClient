
namespace UnitTests
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _responseFactory;
        private readonly List<HttpRequestMessage> _capturedRequests = new();

        public MockHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _capturedRequests.Add(request);
            cancellationToken.ThrowIfCancellationRequested();
            var res = await _responseFactory(request, cancellationToken);
            return res;
        }

        public int CountRequests(Func<HttpRequestMessage, bool> predicate)
        {
            return _capturedRequests.Count(predicate);
        }
    }
}