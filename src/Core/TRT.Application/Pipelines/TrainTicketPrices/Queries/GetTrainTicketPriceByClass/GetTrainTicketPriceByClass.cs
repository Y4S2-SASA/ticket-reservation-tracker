using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRT.Application.DTOs.TrainTicketPriceDTOs;
using TRT.Domain.Enums;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.TrainTicketPrices.Queries.GetTrainTicketPriceByClass
{
    public class GetTrainTicketPriceByClass : IRequest<TrainTicketPriceDTO>
    {
        public PassengerClass PassengerClass { get; set; }
    }

    public class GetTrainTicketPriceByClassHandler : IRequestHandler<GetTrainTicketPriceByClass, TrainTicketPriceDTO>
    {
        private readonly ITrainTicketPriceQueryRepository _trainTicketPriceQueryRepository;
        public GetTrainTicketPriceByClassHandler(ITrainTicketPriceQueryRepository trainTicketPriceQueryRepository)
        {
            this._trainTicketPriceQueryRepository = trainTicketPriceQueryRepository;
        }
        public async Task<TrainTicketPriceDTO> Handle(GetTrainTicketPriceByClass request, CancellationToken cancellationToken)
        {
            var ticketPrice = (await _trainTicketPriceQueryRepository.Query(x=>x.PassengerClass == request.PassengerClass))
                              .FirstOrDefault();

            return new TrainTicketPriceDTO()
            {
                Id = ticketPrice.Id,
                PassengerClass = ticketPrice.PassengerClass,
                Price = ticketPrice.Price,
            };
        }
    }
}
