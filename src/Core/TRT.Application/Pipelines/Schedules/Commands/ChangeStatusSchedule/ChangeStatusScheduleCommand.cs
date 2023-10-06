using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Schedules.Commands.ChangeStatusSchedule
{
    public class ChangeStatusScheduleCommand : IRequest<ResultDTO>
    {
        public StatusChangeDTO StatusChangeDTO { get; set; }
    }

    public class ChangeStatusScheduleCommandHandler : IRequestHandler<ChangeStatusScheduleCommand, ResultDTO>
    {
        private readonly IScheduleQueryRepository _scheduleQueryRepository;
        private readonly IScheduleCommandRepository _scheduleCommandRepository;
        private readonly ILogger<ChangeStatusScheduleCommandHandler> _logger;   
        public ChangeStatusScheduleCommandHandler
        (
            IScheduleQueryRepository _scheduleQueryRepository,
            IScheduleCommandRepository _scheduleCommandRepository,
            ILogger<ChangeStatusScheduleCommandHandler> logger

        )
        {
            this._scheduleQueryRepository = _scheduleQueryRepository;
            this._scheduleCommandRepository = _scheduleCommandRepository;
            this._logger = logger;  
        }
        public async Task<ResultDTO> Handle(ChangeStatusScheduleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var schedule = await _scheduleQueryRepository.GetById(request.StatusChangeDTO.Id,cancellationToken);

                schedule.Status = request.StatusChangeDTO.Status;

                await _scheduleCommandRepository.UpdateAsync(schedule, cancellationToken);

                return ResultDTO.Success(ResponseMessageConstant.SCHEDULE_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);

                throw new ApplicationException(ResponseMessageConstant.COMMON_EXCEPTION_RESPONSE_MESSAGE);
            }
        }
    }
}
