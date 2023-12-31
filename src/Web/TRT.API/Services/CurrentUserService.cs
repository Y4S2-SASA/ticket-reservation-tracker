﻿using System.Security.Claims;
using TRT.Application.Common.Interfaces;
/*
 * File: CurrentUserService.cs
 * Purpose: Handle Current user
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public string? UserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    return null;

                var user = _httpContextAccessor.HttpContext.User;

                if (user == null)
                    return null;

                return user.FindFirstValue(ClaimTypes.NameIdentifier);


            }
        }
    }
}
