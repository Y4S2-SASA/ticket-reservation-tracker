using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Application.Pipelines.Trains.Commands.SaveTrain;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Trains.Commands.ChangeTrainStatus
{
    public class ChangeTrainStatusCommand : IRequest<ResultDTO>
    {
        public string Id { get; set; }
        public Status  Status { get; set; }
    }

    public class ChangeTrainStatusCommandHandler : IRequestHandler<ChangeTrainStatusCommand, ResultDTO>
    {
        private readonly ITrainQueryRepository _trainQueryRepository;
        private readonly ITrainCommandRepository _trainCommandRepository;
        private readonly ILogger<ChangeTrainStatusCommandHandler> _logger;
        public ChangeTrainStatusCommandHandler
        (
            ITrainQueryRepository trainQueryRepository,
            ITrainCommandRepository trainCommandRepository,
            ILogger<ChangeTrainStatusCommandHandler> logger
        )
        {
            this._trainQueryRepository = trainQueryRepository;
            this._trainCommandRepository = trainCommandRepository;
            this._logger = logger;
        }
        public async Task<ResultDTO> Handle(ChangeTrainStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var train = await _trainQueryRepository.GetById(request.Id, cancellationToken);

                if (train != null)
                {
                    train.Status = request.Status;

                    await _trainCommandRepository.UpdateAsync(train, cancellationToken);

                    return ResultDTO.Success
                        (
                          string.Format(ResponseMessageConstant.TRAIN_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE,
                                train.TrainName,
                                EnumHelper.GetEnumDescription(request.Status)
                         ));
                }
                else
                {
                    return ResultDTO.Failure(new List<string>()
                    {
                        ResponseMessageConstant.TRAIN_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE
                    });
                }

               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
