using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;
/*
 * File: ChangeTrainStatusCommand.cs
 * Purpose: Handle Change Train Status
 * Author: Jayathilake S.M.D.A.R/IT20037338
*/
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
        private readonly IReservationQueryRepository _reservationQueryRepository;
        private readonly ILogger<ChangeTrainStatusCommandHandler> _logger;
        public ChangeTrainStatusCommandHandler
        (
            ITrainQueryRepository trainQueryRepository,
            ITrainCommandRepository trainCommandRepository,
            IReservationQueryRepository reservationQueryRepository,
            ILogger<ChangeTrainStatusCommandHandler> logger
        )
        {
            this._trainQueryRepository = trainQueryRepository;
            this._trainCommandRepository = trainCommandRepository;
            this._reservationQueryRepository = reservationQueryRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Handle Change Train Status
        /// </summary>
        /// <param name="request">>Contains Status change parameter details</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>ResultDTO</returns>
        public async Task<ResultDTO> Handle(ChangeTrainStatusCommand request, CancellationToken cancellationToken)
        {
           
                var train = await _trainQueryRepository.GetById(request.Id, cancellationToken);

                if (train != null)
                {
                    var reservations = (await _reservationQueryRepository.Query(x => x.TrainId == request.Id)).ToList();
                    
                    if(reservations.Count > NumberConstant.ONE)
                    {
                        return ResultDTO.Failure(new List<string>()
                        {
                            ResponseMessageConstant.CANNOT_TRAIN_STATUS_CHANGE
                        });
                    }
                    else
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

                    
                }
                else
                {
                    return ResultDTO.Failure(new List<string>()
                    {
                        ResponseMessageConstant.TRAIN_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE
                    });
                }

               
           
        }
    }
}
