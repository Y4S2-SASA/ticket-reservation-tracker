using MediatR;
using TRT.Application.DTOs.TrainDTOs;
using TRT.Domain.Repositories.Query;
/*
 * File: GetTrainByIdQuery.cs
 * Purpose: Handle GetScheduleById
 * Author: Jayathilake S.M.D.A.R/IT20037338
*/
namespace TRT.Application.Pipelines.Trains.Queries.GetTrainById
{
    public record GetTrainByIdQuery(string id) : IRequest<TrainDTO>
    {
    }

    public class GetGetTrainByIdQueryHandler : IRequestHandler<GetTrainByIdQuery, TrainDTO>
    {
        private readonly ITrainQueryRepository _trainQueryRepository;

        public GetGetTrainByIdQueryHandler
        (
            ITrainQueryRepository trainQueryRepository
        )
        {
            _trainQueryRepository = trainQueryRepository;

        }
        /// <summary>
        /// Handle Save and update TrainCommand.
        /// </summary>
        /// <param name="request">>Contains TrainId</param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>Train details</returns>
        public async Task<TrainDTO> Handle(GetTrainByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var train = await _trainQueryRepository.GetById(request.id, cancellationToken);

                return train.ToDto();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
