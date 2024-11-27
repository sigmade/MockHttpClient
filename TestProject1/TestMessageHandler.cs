namespace UnitTests
{
    internal class TestMessageHandler : HttpMessageHandler
    {
        private Queue<HttpResponseMessage> _responseMessages;

        public TestMessageHandler()
        {
            _responseMessages = new Queue<HttpResponseMessage>();
        }

        internal void SetResponse(HttpResponseMessage responseMessage)
        {
            _responseMessages.Enqueue(responseMessage);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_responseMessages.Count > 0)
            {
                return Task.FromResult(_responseMessages.Dequeue());
            }

            throw new InvalidOperationException("No response message available.");
        }
    }
}