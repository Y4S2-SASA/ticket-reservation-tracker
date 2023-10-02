using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.TrainDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Trains.Queries.GetTrainsByFilter
{
    public record GetTrainsByFilterQuery : IRequest<PaginatedListDTO<TrainDetailDTO>>
    {
        public string? SearchText { get; set; }

        public Status Status { get; set; }
        public AvailableDays AvailableDay { get; set; }
        public PassengerClass PassengerClass { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class GetTrainsByFilterQueryHandler : IRequestHandler<GetTrainsByFilterQuery, PaginatedListDTO<TrainDetailDTO>>
    {
        private readonly ITrainQueryRepository _trainQueryRepository;
        private readonly ILogger<GetTrainsByFilterQueryHandler> _logger;
        public GetTrainsByFilterQueryHandler
        (
            ITrainQueryRepository trainQueryRepository,
            ILogger<GetTrainsByFilterQueryHandler> logger)

        {
            _trainQueryRepository = trainQueryRepository;
            _logger = logger;
        }
        public async Task<PaginatedListDTO<TrainDetailDTO>> Handle(GetTrainsByFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalRecordCount = NumberConstant.ZERO;

                Expression<Func<Train, bool>> query = x => x.Status != Status.Deleted;

                query = ConfigureFilter(query, request);

                totalRecordCount = (int)await _trainQueryRepository.CountDocumentsAsync(query);

                var availableData = await _trainQueryRepository.GetPaginatedDataAsync
                                   (query,
                                       request.PageSize,
                                       request.CurrentPage,
                                       cancellationToken
                                   );

                var listOfTrains = availableData.OrderByDescending(x => x.TrainName)
                                  .Select(x => x.ToDetailDto())
                                  .ToList();

                return new PaginatedListDTO<TrainDetailDTO>
                      (
                          listOfTrains,
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

        private Expression<Func<Train, bool>> ConfigureFilter(Expression<Func<Train, bool>> query, GetTrainsByFilterQuery request)
        {
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = x => x.TrainName.Contains(request.SearchText);
            }

            if (request.Status > NumberConstant.ZERO)
            {
                query = x => x.Status == request.Status;
            }

            if (request.AvailableDay > NumberConstant.ZERO)
            {
                query = x => x.AvailableDays == request.AvailableDay;
            }

            if (request.PassengerClass > NumberConstant.ZERO)
            {
                query = x => x.PassengerClasses.Any(a => a == request.PassengerClass);
            }

            return query;
        }
    }
}
