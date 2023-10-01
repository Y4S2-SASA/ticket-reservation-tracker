using MediatR;
using TRT.Application.DTOs.TrainDTOs;
using TRT.Domain.Repositories.Query;

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
