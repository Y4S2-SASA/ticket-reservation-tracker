using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.TrainDTOs;
using TRT.Domain.Enums;

namespace TRT.Application.Pipelines.Trains.Queries.GetTrainDetailMasterData
{
    public class GetTrainDetailMasterDataQuery : IRequest<TrainMasterDataDTO>
    {
    }

    public class GetTrainDetailMasterDataQueryHandler : IRequestHandler<GetTrainDetailMasterDataQuery, TrainMasterDataDTO>
    {
        private readonly IMediator _mediator;
        private ILogger<GetTrainDetailMasterDataQueryHandler> _logger;
        public GetTrainDetailMasterDataQueryHandler
        (
            IMediator mediator,
            ILogger<GetTrainDetailMasterDataQueryHandler> logger
        )
        {
            this._mediator = mediator;
            this._logger = logger;
        }
        public async Task<TrainMasterDataDTO> Handle(GetTrainDetailMasterDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var trainDetailMasterData = new TrainMasterDataDTO();

                trainDetailMasterData.Status = Enum.GetValues(typeof(Status))
                         .Cast<Status>()
                         .Select(x => new DropDownDTO
                         {
                             Id = (int)x,
                             Name = EnumHelper.GetEnumDescription(x),
                         })
                         .ToList();

                trainDetailMasterData.AvailableDays = Enum.GetValues(typeof(AvailableDays))
                         .Cast<AvailableDays>()
                         .Select(x => new DropDownDTO
                         {
                             Id = (int)x,
                             Name = EnumHelper.GetEnumDescription(x),
                         })
                         .ToList();

                trainDetailMasterData.PassengerClasses = Enum.GetValues(typeof(PassengerClass))
                         .Cast<PassengerClass>()
                         .Select(x => new DropDownDTO
                         {
                             Id = (int)x,
                             Name = EnumHelper.GetEnumDescription(x),
                         })
                         .ToList();

                var defaultValue = new DropDownDTO()
                {
                    Id = 0,
                    Name = "-All-"
                };

                trainDetailMasterData.Status.Insert(0, defaultValue);
                trainDetailMasterData.AvailableDays.Insert(0, defaultValue);
                trainDetailMasterData.PassengerClasses.Insert(0, defaultValue);
              
                return trainDetailMasterData;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
