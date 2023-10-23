using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Application.DTOs.ScheduleDTOs;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;
/*
 * File: SaveScheduleCommand.cs
 * Purpose: Handle SaveScheduleCommand
 * Author: Perera M.S.D/1IT20020262
*/
namespace TRT.Application.Pipelines.Schedules.Commands.SaveSchedule
{
    public record SaveScheduleCommand : IRequest<ResultDTO>
    {
        public ScheduleDTO ScheduleDTO { get; set; }
    }

    public class SaveScheduleCommandHandler : IRequestHandler<SaveScheduleCommand, ResultDTO>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly IScheduleCommandRepository _scheduleCommandRepository;
        private readonly ILogger<SaveScheduleCommandHandler> _logger;
        public SaveScheduleCommandHandler
        (
            IScheduleQueryRepository scheduleQueryRepository,
            IScheduleCommandRepository scheduleCommandRepository,
            ILogger<SaveScheduleCommandHandler> logger
        )
        {
            this._scheduleQueryRepository = scheduleQueryRepository;
            this._scheduleCommandRepository = scheduleCommandRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Handle ChangeStatusSchedule.
        /// </summary>
        /// <param name="request">>Contains Schedule data</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>Result dto</returns>
        public async Task<ResultDTO> Handle(SaveScheduleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ScheduleDTO.Id))
                {
                    var schedule = new Schedule();

                    schedule.DepartureStationId = request.ScheduleDTO.DepartureStationId;
                    schedule.ArrivalStationId = request.ScheduleDTO.ArrivalStationId;
                    schedule.DepartureTime = request.ScheduleDTO.DepartureTime;
                    schedule.ArrivalTime = request.ScheduleDTO.ArrivalTime;
                    schedule.TrainId = request.ScheduleDTO.TrainId;
                    schedule.Status = Domain.Enums.Status.Pending;

                    AddNewSubStations(schedule, request.ScheduleDTO.SubStationDetails);

                    schedule.SubStationDetails.Add(new SubStationDetail()
                    {
                       
                        StationId = request.ScheduleDTO.ArrivalStationId,
                        ArrivalTime = request.ScheduleDTO.ArrivalTime,
                    });

                    schedule.SubStationDetails.Insert(NumberConstant.ZERO, new SubStationDetail()
                    {
                        StationId = request.ScheduleDTO.DepartureStationId,
                        ArrivalTime = request.ScheduleDTO.DepartureTime,
                    });



                    await _scheduleCommandRepository.AddAsync(schedule, cancellationToken);

                    return ResultDTO.Success(ResponseMessageConstant.SCHEDULE_SAVE_SUCCESS_RESPONSE_MESSAGE);

                }
                else
                {
                    var schedule = await _scheduleQueryRepository.GetById(request.ScheduleDTO.Id, cancellationToken);

                    schedule.DepartureStationId = request.ScheduleDTO.DepartureStationId;
                    schedule.ArrivalStationId = request.ScheduleDTO.ArrivalStationId;
                    schedule.DepartureTime = request.ScheduleDTO.DepartureTime;
                    schedule.ArrivalTime = request.ScheduleDTO.ArrivalTime;
                    schedule.TrainId = request.ScheduleDTO.TrainId;

                    var exsistingSubstations = schedule.SubStationDetails.ToList();
                    var selectedSubstations = request.ScheduleDTO.SubStationDetails.ToList();

                    var newSubstations = (from station in selectedSubstations where exsistingSubstations
                                          .All(x=>x.StationId != station.StationId) select station)
                                          .ToList();


                    var deletedSubstations = (from station in exsistingSubstations where selectedSubstations
                                              .All(x => x.StationId != station.StationId) select station)
                                              .ToList();

                    AddNewSubStations(schedule, newSubstations);

                    foreach (var item in deletedSubstations)
                    {
                        schedule.SubStationDetails.Remove(item);
                    }

                    await _scheduleCommandRepository.UpdateAsync(schedule, cancellationToken);

                    return ResultDTO.Success(ResponseMessageConstant.SCHEDULE_UPDATE_SUCCESS_RESPONSE_MESSAGE);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        //AddNewSubStations
        private void AddNewSubStations(Schedule schedule, List<SubStationDetailDTO> subStationDetails)
        {
            foreach (var item in subStationDetails)
            {
                schedule.SubStationDetails.Add(new SubStationDetail()
                {
                    StationId = item.StationId,
                    ArrivalTime = item.ArrivalTime,
                });
            }
        }
    }
}
