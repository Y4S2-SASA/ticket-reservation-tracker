using MediatR;
using Microsoft.Extensions.Logging;
using TRT.Application.Common.Constants;
using TRT.Application.Common.Helpers;
using TRT.Application.DTOs.Common;
using TRT.Application.DTOs.TrainDTOs;
using TRT.Domain.Enums;
/*
 * File: GetTrainDetailMasterDataQuery.cs
 * Purpose: Handle Get Train Detail Master Data
 * Author: Jayathilake S.M.D.A.R/IT20037338
*/
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

        /// <summary>
        /// Handle  Get Train Detail Master Data
        /// </summary>
        /// <param name="request">></param>
        /// <param name="cancellationToken">>The token to monitor for cancellation requests</param>
        /// <returns>Train Maste rData</returns>
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
                    Id = NumberConstant.ZERO,
                    Name = "-All-"
                };

                trainDetailMasterData.Status.Insert(NumberConstant.ZERO, defaultValue);
                trainDetailMasterData.AvailableDays.Insert(NumberConstant.ZERO, defaultValue);
                trainDetailMasterData.PassengerClasses.Insert(NumberConstant.ZERO, defaultValue);
              
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
