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

/*
 * File: GetSchedulesByFilterQuery.cs
 * Purpose: Handle  Get Schedules filter
 * Author: Perera M.S.D/IT20020262
*/
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

        /// <summary>
        /// Handle GetScheduleById.
        /// </summary>
        /// <param name="request">>Parameters</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>PaginatedListDTO<ScheduleDetailDTO></returns>
        public async Task<PaginatedListDTO<ScheduleDetailDTO>> Handle(GetSchedulesByFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalRecordCount = NumberConstant.ZERO;

                var scheduleDetails = new List<ScheduleDetailDTO>();

                Expression<Func<Schedule, bool>> query = x => true;


                var availableData = (await _scheduleQueryRepository.Query(x=>x.TrainId == request.TrainId))
                                    .ToList();

                if (!string.IsNullOrEmpty(request.ArrivalStationId))
                {
                    availableData = (await _scheduleQueryRepository.Query(x => x.TrainId == request.TrainId &&
                                    x.ArrivalStationId == request.ArrivalStationId))
                                    .ToList();
                }

                if(!string.IsNullOrEmpty(request.DepartureStationId))
                {
                    availableData = (await _scheduleQueryRepository.Query(x => x.TrainId == request.TrainId &&
                                   x.DepartureStationId == request.DepartureStationId))
                                   .ToList();
                }

                if(!string.IsNullOrEmpty(request.ArrivalStationId) && !string.IsNullOrEmpty(request.DepartureStationId))
                {
                    availableData = (await _scheduleQueryRepository.Query(x => x.TrainId == request.TrainId &&
                                     x.DepartureStationId == request.DepartureStationId && 
                                     x.ArrivalStationId == request.ArrivalStationId))
                                  .ToList();
                }

                totalRecordCount = (int)availableData.Count();

                availableData = availableData.Skip(request.CurrentPage * request.PageSize)
                                            .Take(request.PageSize)
                                            .ToList();

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

                return new PaginatedListDTO<ScheduleDetailDTO>
                       (
                           new List<ScheduleDetailDTO>(),
                           NumberConstant.ZERO,
                           NumberConstant.ZERO,
                           NumberConstant.ZERO
                       );
            }
        }
    }
}
