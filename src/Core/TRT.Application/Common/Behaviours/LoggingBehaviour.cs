using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Interfaces;
/*
 * File: LoggingBehaviour.cs
 * Author: Perera M.S.D/IT20020262
 */

namespace TRT.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }
        //loggin Behaviour
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;

            _logger.LogInformation("Request: {Name} {@UserId} {@Request}",
                requestName, userId, request);
        }
    }
}
