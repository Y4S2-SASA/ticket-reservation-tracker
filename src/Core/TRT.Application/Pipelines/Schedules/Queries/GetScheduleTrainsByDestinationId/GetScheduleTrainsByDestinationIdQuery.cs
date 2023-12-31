﻿using MediatR;
using TRT.Application.DTOs.Common;
using TRT.Domain.Repositories.Query;
/*
 * File: GetScheduleTrainsByDestinationIdQuery.cs
 * Purpose: Handle  Get Schedule Trains By Destination Id
 * Author: Perera M.S.D/IT20020262
*/
namespace TRT.Application.Pipelines.Schedules.Queries.GetScheduleTrainsByDestinationId
{
    public record GetScheduleTrainsByDestinationIdQuery(string destinationId) : IRequest<List<DropDownCoreDTO>>
    {
    }

    public class GetScheduleTrainsByDestinationIdQueryHandler :
                                            IRequestHandler<GetScheduleTrainsByDestinationIdQuery, List<DropDownCoreDTO>>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly ITrainQueryRepository _trainQueryRepository;

        public GetScheduleTrainsByDestinationIdQueryHandler
        (
            IScheduleQueryRepository scheduleQueryRepository,
            ITrainQueryRepository trainQueryRepository
        )
        {
            this._scheduleQueryRepository = scheduleQueryRepository;
            this._trainQueryRepository = trainQueryRepository;
        }


        /// <summary>
        /// Handle Get Schedule Trains By Destination Id.
        /// </summary>
        /// <param name="request">>Contains destinationId</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>Schedule Trains  Destination Master Data</returns>
        public async Task<List<DropDownCoreDTO>> Handle(GetScheduleTrainsByDestinationIdQuery request, CancellationToken cancellationToken)
        {
            var trains = new List<DropDownCoreDTO>();

            var listOfSchedules = (await _scheduleQueryRepository
                              .Query(x =>x.Status == Domain.Enums.Status.Activated &&  
                               x.SubStationDetails.Any(d => d.StationId == request.destinationId)))
                              .ToList();

            foreach ( var item in listOfSchedules)
            {
                var train = await _trainQueryRepository.GetById(item.TrainId, cancellationToken);

                trains.Add(new DropDownCoreDTO()
                {
                    Id = train.Id,
                    Name = train.TrainName
                });
            }

            return trains;
        }
    }
}
