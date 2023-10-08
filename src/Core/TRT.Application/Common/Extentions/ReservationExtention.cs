using TRT.Application.DTOs.ReservationDTOs;
using TRT.Domain.Entities;

namespace TRT.Application.Common.Extentions
{
    public static class ReservationExtention
    {
        public static Reservation ToEntity(this ReservationDTO reservationDTO, Reservation? reservation = null)
        {
            if(reservation is null) reservation = new Reservation();

            reservation.PassengerClass = reservationDTO.PassengerClass;
            reservation.DestinationStationId = reservationDTO.DestinationStationId;
            reservation.TrainId = reservationDTO.TrainId;
            reservation.ArrivalStationId = reservationDTO.ArrivalStationId;
            reservation.DateTime = reservationDTO.DateTime;
            reservation.NoOfPassengers = reservationDTO.NoOfPassengers;
            reservation.Price = reservationDTO.Price;

            return reservation;
        }
    }
}
