﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Interfaces;
/*
 * File: PerformanceBehaviour.cs
 * Author: Jayathilake S.M.D.A.R/IT20037338
 */

namespace TRT.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService
         )
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserService = currentUserService;
        }
        //handle PerformanceBehaviour
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= NumberConstant.FIVEHUNDRANT) return response;
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;


            _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
                requestName, elapsedMilliseconds, userId, request);

            return response;
        }
    }
}
