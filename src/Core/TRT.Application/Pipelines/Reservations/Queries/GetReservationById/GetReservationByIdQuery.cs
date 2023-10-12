using MediatR;
using TRT.Application.Common.Extentions;
using TRT.Application.DTOs.ReservationDTOs;
using TRT.Domain.Repositories.Query;

namespace TRT.Application.Pipelines.Reservations.Queries.GetReservationById
{
    public record GetReservationByIdQuery(string id) : IRequest<ReservationDTO>;


    public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ReservationDTO>
    {
        private readonly IReservationQueryRepository _reservationQueryRepository;
        public GetReservationByIdQueryHandler(IReservationQueryRepository reservationQueryRepository)
        {     
            this._reservationQueryRepository = reservationQueryRepository;
        }
        public async Task<ReservationDTO> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationQueryRepository.GetById(request.id, cancellationToken);

           return reservation.ToDto();

        }
    }
}
