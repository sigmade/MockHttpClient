using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebApi.Middleware;

namespace UnitTests;

public class MiddlewareTests
{
    [Fact]
    public async Task ShouldLogErrorAndReturn500()
    {
        var logger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();
        var exception = new InvalidOperationException("Test exception");
        var wasExecuted = false;

        Task next(HttpContext context)
        {
            wasExecuted = true;
            throw exception;
        }

        var middleware = new ExceptionHandlingMiddleware(next, logger);
        var httpContext = new DefaultHttpContext();

        await middleware.InvokeAsync(httpContext);

        Assert.True(wasExecuted);
        Assert.Equal(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);

        logger.Received(1).LogError(
            Arg.Any<EventId>(),
            exception,
            "An error occurred while processing the request"
        );
    }



    [Fact]
    public async Task NoExceptionThrown_ShouldNotReturn500()
    {
        var logger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();
        var wasExecuted = false;

        Task next(HttpContext context)
        {
            wasExecuted = true;
            context.Response.StatusCode = StatusCodes.Status200OK;
            return Task.CompletedTask;
        }

        var middleware = new ExceptionHandlingMiddleware(next, logger);
        var httpContext = new DefaultHttpContext();

        await middleware.InvokeAsync(httpContext);

        Assert.True(wasExecuted);
        Assert.NotEqual(StatusCodes.Status500InternalServerError, httpContext.Response.StatusCode);

        logger.DidNotReceive().LogError(
            Arg.Any<EventId>(),
            Arg.Any<Exception>(),
            "An error occurred while processing the request"
        );
    }
}
