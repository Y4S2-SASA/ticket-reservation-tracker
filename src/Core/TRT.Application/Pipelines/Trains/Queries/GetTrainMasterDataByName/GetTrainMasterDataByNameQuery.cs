using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TRT.Application.DTOs.Common;
using TRT.Domain.Entities;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Trains.Queries.GetTrainMasterDataByName
{
    public class GetTrainMasterDataByNameQuery : IRequest<List<DropDownCoreDTO>>
    {
        public string? SearchText { get; set; }
    }

    public class GetTrainMasterDataByNameQueryHandler : IRequestHandler<GetTrainMasterDataByNameQuery, List<DropDownCoreDTO>>
    {
        private readonly ITrainQueryRepository _trainQueryRepository;
        private readonly ILogger<GetTrainMasterDataByNameQueryHandler> _logger;
        public GetTrainMasterDataByNameQueryHandler
        (
            ITrainQueryRepository trainQueryRepository, 
            ILogger<GetTrainMasterDataByNameQueryHandler> _logger
        )
        {
            this._trainQueryRepository = trainQueryRepository;
            this._logger = _logger;
        }
        public async Task<List<DropDownCoreDTO>> Handle(GetTrainMasterDataByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Train, bool>> query = x => x.Status == Domain.Enums.Status.Activated;

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query = x=> x.TrainName.Contains(request.SearchText);
                }

                var trains = (await _trainQueryRepository.Query(query))
                            .Take(10).ToList();

                return trains.Select(x => new DropDownCoreDTO()
                {
                    Id = x.Id,
                    Name = x.TrainName
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
