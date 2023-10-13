using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Data;
using System.Linq.Expressions;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;
/*
 * File: GetAllUsersByFilterQuery.cs
 * Purpose: Handle Get All Users By Filter 
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Application.Pipelines.Users.Queries.GetAllUsersByFilter
{
    public record GetAllUsersByFilterQuery : IRequest<PaginatedListDTO<UserDetailDTO>>
    {
        public string? SearchText { get; set; }
        public Status  Status { get; set; }
        public Role Role { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllUsersByFilterQueryHandler : IRequestHandler<GetAllUsersByFilterQuery, PaginatedListDTO<UserDetailDTO>>
    {
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly ILogger<GetAllUsersByFilterQueryHandler> _logger;
        public GetAllUsersByFilterQueryHandler
        (
            IUserQueryRepository userQueryRepository,
            ILogger<GetAllUsersByFilterQueryHandler> logger
        )
        {
            this._userQueryRepository = userQueryRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Handle Get All Users By Filter user
        /// </summary>
        /// <param name="request">>Contains filter parameters </param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>Paginated user list</returns>
        public async Task<PaginatedListDTO<UserDetailDTO>> Handle(GetAllUsersByFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalRecordCount = NumberConstant.ZERO;

                Expression<Func<User, bool>> query = x => true;

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query = x => x.FirstName.ToLower().Trim().Contains(request.SearchText.ToLower().Trim()) ||
                                 x.LastName.ToLower().Trim().Contains(request.SearchText.ToLower().Trim()) ||
                                 x.UserName.ToLower().Trim().Contains(request.SearchText.ToLower().Trim());
                }

                if(request.Status > NumberConstant.ZERO)
                {
                    query = x => x.Status == request.Status;
                }

                if(request.Role > NumberConstant.ZERO)
                {
                   query = x => x.Role == request.Role;
                }

                totalRecordCount = (int)await _userQueryRepository.CountDocumentsAsync(query);

                var availableData = await _userQueryRepository.GetPaginatedDataAsync
                                    (   query,
                                        request.PageSize, 
                                        request.CurrentPage, 
                                        cancellationToken
                                    );

                var listOfUsers = availableData.OrderByDescending(x => x.FirstName)
                                  .Select(x => x.ToDetailDto())
                                  .ToList();

                return new PaginatedListDTO<UserDetailDTO>
                        (
                            listOfUsers, 
                            totalRecordCount, 
                            request.CurrentPage + ApplicationLevelConstant.PAGINATION_PAGE_INCREMENT, 
                            request.PageSize
                        );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
