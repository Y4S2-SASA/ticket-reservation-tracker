using MediatR;
using TRT.Application.DTOs.Common;
using TRT.Application.Pipelines.Stations.Queries.GetStatusByIdQuery;
using TRT.Domain.Repositories.Query;

/*
 * File: GetSchudulesSubStationsByTrainIdQuery.cs
 * Purpose: Handle Get Schudules SubStations By TrainId
 * Author: Perera M.S.D/IT20020262
*/
namespace TRT.Application.Pipelines.Schedules.Queries.GetSchudulesSubStationsByTrainId
{
    public record GetSchudulesSubStationsByTrainIdQuery(string trainId) : IRequest<List<DropDownCoreDTO>>
    {
    }

    public class GetSchudulesSubStationsByTrainIdQueryHandler
                : IRequestHandler<GetSchudulesSubStationsByTrainIdQuery, List<DropDownCoreDTO>>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly IMediator _mediator;
        

        public GetSchudulesSubStationsByTrainIdQueryHandler
        (
            IScheduleQueryRepository scheduleQueryRepository,
            IMediator mediator
        )
        {
            this._scheduleQueryRepository = scheduleQueryRepository;
            this._mediator = mediator;
        }

        /// <summary>
        /// Handle Get Schudules Sub Stations.
        /// </summary>
        /// <param name="request">>Contains train  id</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>SubStations master data</returns>
        public async Task<List<DropDownCoreDTO>> Handle(GetSchudulesSubStationsByTrainIdQuery request, CancellationToken cancellationToken)
        {
            var subStations = new List<DropDownCoreDTO>();

            var listOfSchedules = (await _scheduleQueryRepository
                                                     .Query(x => x.Status == Domain.Enums.Status.Activated && 
                                                     x.TrainId == request.trainId)).ToList();

            foreach (var schedule in listOfSchedules)
            {
                foreach(var station in  schedule.SubStationDetails) 
                {
                    var subStation = await _mediator
                                           .Send(new GetStationByIdQuery(station.StationId));

                    subStations.Add(new DropDownCoreDTO()
                    {
                        Id = subStation.Id,
                        Name = subStation.Name,
                    });
                }
            }

            return subStations;
        }
    }
}
