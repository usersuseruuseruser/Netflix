﻿
using Application.Exceptions;
using Domain.Services.ServiceExceptions;
namespace API.Middlewares.ExceptionHandler
{
    public class ExceptionHandlerMiddleware() : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : this()
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ArgumentException ex) when (
                ex is ReviewServiceArgumentException ||
                ex is FavouriteServiceArgumentException ||
                ex is ContentServiceArgumentException ||
                ex is CommentServiceArgumentException ||
                ex is NotificationServiceArgumentException ||
                ex is SubscriptionServiceArgumentException ||
                ex is UserServiceArgumentException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = $"{ex.Message}",
                    Code = 400
                });
                _logger.LogError(ex.Message + ex.StackTrace);
            }
            catch (ContentServiceNotPermittedException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = ex.Message,
                    Code = 403
                });
                _logger.LogError(ex.Message + ex.StackTrace);

            }
            // TODO: бизнес ошибки отправляются, нужно 500
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ExceptionDetails
                {
                    Message = ex.Message,
                    Code = 400
                });
                _logger.LogError(ex.Message + ex.StackTrace);
            }
        }
        private class ExceptionDetails
        {
            public string Message { get; set; } = null!;
            public int Code { get; set; }
        }
    }
}
