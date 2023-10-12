using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.ScheduleDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Schedules.Queries.GetSchedulesByFilter
{
    public record GetSchedulesByFilterQuery 
                    : IRequest<PaginatedListDTO<ScheduleDetailDTO>>
    {
        public string? TrainId { get; set; }
        public string? DepartureStationId { get; set; }
        public string? ArrivalStationId { get; set; }
        public Status Status { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class GetSchedulesByFilterQueryHandler 
        : IRequestHandler<GetSchedulesByFilterQuery, PaginatedListDTO<ScheduleDetailDTO>>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly ITrainQueryRepository _trainQueryRepository;
        private readonly IStationQueryRepository _stationQueryRepository;
        private readonly ILogger<GetSchedulesByFilterQueryHandler> _logger;
        public GetSchedulesByFilterQueryHandler
        (
            IScheduleQueryRepository scheduleQueryRepository,
            ITrainQueryRepository trainQueryRepository,
            IStationQueryRepository stationQueryRepository,
            ILogger<GetSchedulesByFilterQueryHandler> logger
        )
        {
            this._scheduleQueryRepository = scheduleQueryRepository;
            this._trainQueryRepository = trainQueryRepository;
            this._stationQueryRepository = stationQueryRepository;
            this._logger = logger;
        }
        public async Task<PaginatedListDTO<ScheduleDetailDTO>> Handle(GetSchedulesByFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalRecordCount = NumberConstant.ZERO;

                var scheduleDetails = new List<ScheduleDetailDTO>();

                Expression<Func<Schedule, bool>> query = x => true;

                if(!string.IsNullOrEmpty(request.TrainId))
                {
                    query = x => x.TrainId == request.TrainId;
                }

                if(!string.IsNullOrEmpty(request.DepartureStationId))
                {
                    query = x => x.DepartureStationId == request.DepartureStationId;
                }

                if (!string.IsNullOrEmpty(request.ArrivalStationId))
                {
                    query = x => x.ArrivalStationId == request.ArrivalStationId;
                }

                if(request.Status > NumberConstant.ZERO)
                {
                    query = x => x.Status == request.Status;
                }

                totalRecordCount = (int)await _scheduleQueryRepository.CountDocumentsAsync(query);

                var availableData = await _scheduleQueryRepository.GetPaginatedDataAsync
                                   (
                                       query,
                                       request.PageSize,
                                       request.CurrentPage,
                                       cancellationToken
                                   );

                foreach (var item in availableData ) 
                {
                    var scheduleData = new ScheduleDetailDTO();

                    var train = await _trainQueryRepository.GetById(item.TrainId, cancellationToken);
                    var departureStation = await _stationQueryRepository.GetById(item.DepartureStationId, cancellationToken);
                    var arrivalStationName = await _stationQueryRepository.GetById(item.ArrivalStationId, cancellationToken);

                    scheduleData.Id  = item.Id;
                    scheduleData.TrainName = train.TrainName;
                    scheduleData.DepartureStationName = departureStation.Name;
                    scheduleData.ArrivalStationName = arrivalStationName.Name;
                    scheduleData.ArrivalTime = item.ArrivalTime.ToString(DateTimeFormatConstant.DATE_WITH_TIME_FORMAT);
                    scheduleData.DepartureTime = item.DepartureTime.ToString(DateTimeFormatConstant.DATE_WITH_TIME_FORMAT);
                    scheduleData.Status = EnumHelper.GetEnumDescription(item.Status);
                    scheduleDetails.Add(scheduleData);
                    
                }

                return new PaginatedListDTO<ScheduleDetailDTO>
                       (
                           scheduleDetails,
                           totalRecordCount,
                           request.CurrentPage + ApplicationLevelConstant.PAGINATION_PAGE_INCREMENT,
                           request.PageSize
                       );


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }
    }
}
