using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.DTOs.ScheduleDTOs;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Schedules.Queries.GetScheduleById
{
    public record GetScheduleByIdQuery(string id) : IRequest<ScheduleDTO>;
  
    public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ScheduleDTO>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly ILogger<GetScheduleByIdQueryHandler> _logger;

        public GetScheduleByIdQueryHandler
        (
            IScheduleQueryRepository scheduleQueryRepository,
            ILogger<GetScheduleByIdQueryHandler> logger
        )
        {
            this._scheduleQueryRepository = scheduleQueryRepository;
            this._logger = logger;
        }
        public async Task<ScheduleDTO> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var schedule = await _scheduleQueryRepository.GetById(request.id, cancellationToken);

                var scheduleDTO = new ScheduleDTO();

                scheduleDTO.Id = schedule.Id;
                scheduleDTO.TrainId = schedule.TrainId;
                scheduleDTO.DepartureStationId = schedule.DepartureStationId;
                scheduleDTO.ArrivalStationId = schedule.ArrivalStationId;
                scheduleDTO.DepartureTime = schedule.DepartureTime;
                scheduleDTO.ArrivalTime = schedule.ArrivalTime;

                foreach (var item in schedule.SubStationDetails)
                {
                    var subStation = new SubStationDetailDTO()
                    {
                        StationId = item.StationId,
                        ArrivalTime = item.ArrivalTime,
                    };

                    scheduleDTO.SubStationDetails.Add(subStation);
                }

                return scheduleDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
