using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebApi.Middleware;

namespace UnitTests;

public class MiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ExceptionThrown_ShouldLogErrorAndReturn500()
    {
        var logger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();
        var exception = new InvalidOperationException("Test exception");

        Task next(HttpContext context)
        {
            throw exception;
        }

        var middleware = new ExceptionHandlingMiddleware(next, logger);
        var httpContext = new DefaultHttpContext();

        await middleware.InvokeAsync(httpContext);

        Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);

        logger.Received(1).LogError(
            Arg.Any<EventId>(),
            exception,
            "An error occurred while processing the request"
        );
    }
}
