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

        public static ReservationDTO ToDto(this Reservation reservation, ReservationDTO? dto = null)
        {
            if (dto is null) dto = new ReservationDTO();

            dto.Id = reservation.Id;
            dto.ReferenceNumber = reservation.ReferenceNumber;
            dto.PassengerClass = reservation.PassengerClass;
            dto.DestinationStationId = reservation.DestinationStationId;
            dto.TrainId = reservation.TrainId;
            dto.ArrivalStationId = reservation.ArrivalStationId;
            dto.DateTime = reservation.DateTime;
            dto.NoOfPassengers = reservation.NoOfPassengers;
            dto.Price = reservation.Price;

            return dto;
        }
    }
}
