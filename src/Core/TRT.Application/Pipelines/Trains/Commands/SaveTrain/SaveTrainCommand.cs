using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.DTOs.ResponseDTOs;
using TRT.Application.DTOs.TrainDTOs;
using TRT.Domain.Repositories.Command;
using TRT.Domain.Repositories.Query;
/*
 * File: SaveTrainCommand.cs
 * Purpose: Handle GetScheduleById
 * Author: Jayathilake S.M.D.A.R/IT20037338
*/
namespace TRT.Application.Pipelines.Trains.Commands.SaveTrain
{
    public record SaveTrainCommand : IRequest<ResultDTO>
    {
        public TrainDTO TrainDTO { get; set; }
    }

    public class SaveTrainCommandHandler : IRequestHandler<SaveTrainCommand, ResultDTO>
    {
        private readonly ITrainQueryRepository _trainQueryRepository;
        private readonly ITrainCommandRepository _trainCommandRepository;
        private readonly ILogger<SaveTrainCommandHandler> _logger;  
        public SaveTrainCommandHandler
        (
            ITrainQueryRepository trainQueryRepository,
            ITrainCommandRepository trainCommandRepository,
            ILogger<SaveTrainCommandHandler> logger
        )
        {
            this._trainQueryRepository = trainQueryRepository;
            this._trainCommandRepository = trainCommandRepository;  
            this._logger = logger;  
        }

        /// <summary>
        /// Handle Save and update TrainCommand.
        /// </summary>
        /// <param name="request">>Contains Train details</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>ResultDTO</returns>
        public async Task<ResultDTO> Handle(SaveTrainCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if(string.IsNullOrEmpty(request.TrainDTO.Id))
                {
                    var train = new Domain.Entities.Train();
                    train = request.TrainDTO.ToEntity();
                    train.Status = TRT.Domain.Enums.Status.Pending;
                    var newTrain = await _trainCommandRepository.AddAsync(train, cancellationToken);

                    return ResultDTO.Success(ResponseMessageConstant.TRAIN_SAVE_SUCCESS_RESPONSE_MESSAGE, newTrain.Id);

                }
                else
                {
                    var train = await _trainQueryRepository.GetById(request.TrainDTO.Id, cancellationToken);

                    train = request.TrainDTO.ToEntity(train);
                    
                    var existingPassengerClasses = train.PassengerClasses.ToList();
                    var selectedPassengerClasses = request.TrainDTO.PassengerClasses.ToList();

                    var newPassengerClasses = selectedPassengerClasses.Except(existingPassengerClasses)
                                             .ToList();

                    var deletedPassengerClasses = existingPassengerClasses.Except(selectedPassengerClasses)
                                                  .ToList();

                    if (newPassengerClasses.Count > 0)
                    {
                        train.PassengerClasses.AddRange(newPassengerClasses);
                    }

                    foreach(var item in deletedPassengerClasses)
                    {
                        train.PassengerClasses.Remove(item);
                    }
                  
                    await _trainCommandRepository.UpdateAsync(train, cancellationToken);

                    return ResultDTO.Success(
                        ResponseMessageConstant.TRAIN_UPDATE_SUCCESS_RESPONSE_MESSAGE, train.Id
                   );
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
