using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SIWES360.Application.Common.Models
{

    public sealed class PerformanceBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer = new();
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds > 500)
            {
                _logger.LogWarning(
                    "Long Running Request: {Request} ({Elapsed} ms)",
                    typeof(TRequest).Name,
                    _timer.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
