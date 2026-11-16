using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Domain.Exceptions;

namespace OficinaMotos.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ocorreu um erro não tratado: {Message}", exception.Message);

            var statusCode = exception is DomainException or ValidationException
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError;

            var title = statusCode == StatusCodes.Status400BadRequest
                ? "Violação de regra de negócio ou validação"
                : "Erro interno no servidor";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = statusCode == StatusCodes.Status400BadRequest
                    ? exception.Message
                    : $"Ocorreu um erro inesperado: {exception.Message} Tente novamente mais tarde ou contate o suporte."
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}