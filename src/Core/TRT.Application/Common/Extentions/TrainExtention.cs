using TRT.Application.Common.Helpers;
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
            if (string.IsNullOrEmpty(trainDTO.Id))
            {
                train.PassengerClasses = trainDTO.PassengerClasses;
            }

            return train;
        }

        public static TrainDTO ToDto(this Train train , TrainDTO? trainDTO = null)
        {
            if (trainDTO is null) trainDTO = new TrainDTO();

            trainDTO.Id  = train.Id;
            trainDTO.TrainName = train.TrainName;
            trainDTO.AvailableDays = train.AvailableDays;
            trainDTO.SeatCapacity = train.SeatCapacity;
            trainDTO.PassengerClasses = train.PassengerClasses;

            return trainDTO;
        }

        public static TrainDetailDTO ToDetailDto(this Train train, TrainDetailDTO? trainDetailDTO = null)
        {
            if(trainDetailDTO is null) trainDetailDTO = new TrainDetailDTO();

            trainDetailDTO.Id = train.Id;
            trainDetailDTO.TrainName = train.TrainName;
            trainDetailDTO.TrainAvailableDays = EnumHelper.GetEnumDescription(train.AvailableDays);
            trainDetailDTO.Status = train.Status;
            trainDetailDTO.SeatCapacity = train.SeatCapacity;
            trainDetailDTO.PassengerClassNames = string.Join(",", train.PassengerClasses
                                                .Select(x => EnumHelper.GetEnumDescription(x)));
            

            return trainDetailDTO;
        }

       
    }
}
