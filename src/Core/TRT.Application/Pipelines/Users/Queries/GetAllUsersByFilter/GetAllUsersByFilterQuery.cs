using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Data;
using System.Linq.Expressions;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.UserDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;

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
        public async Task<PaginatedListDTO<UserDetailDTO>> Handle(GetAllUsersByFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalRecordCount = 0;

                Expression<Func<User, bool>> query = x => true;

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                   query = x=>x.FirstName.Contains(request.SearchText) || 
                                    x.LastName.Contains(request.SearchText) ||
                                    x.UserName.Contains(request.SearchText);
                }

                if(request.Status > 0)
                {
                    query = x => x.Status == request.Status;
                }

                if(request.Role > 0)
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
                                  .Select(x => x.ToDto())
                                  .ToList();

                return new PaginatedListDTO<UserDetailDTO>
                        (
                            listOfUsers, 
                            totalRecordCount, 
                            request.CurrentPage + 1, 
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
