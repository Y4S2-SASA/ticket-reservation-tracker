using TRT.Application.DTOs.TrainDTOs;
using TRT.Domain.Entities;

namespace System
{
    public static class TrainExtention
    {
        public static Train ToEntity(this TrainDTO trainDTO, Train? train = null)
        {
            if (train is null) train = new Train();

            train.TrainName = trainDTO.TrainName;
            train.AvailableDays = trainDTO.AvailableDays;
            train.SeatCapacity = trainDTO.SeatCapacity;
            train.PassengerClasses = trainDTO.PassengerClasses;
           

            return train;
        }

       
    }
}
