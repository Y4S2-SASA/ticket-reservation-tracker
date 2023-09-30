using TRT.Domain.Enums;

namespace TRT.Application.DTOs.TrainDTOs
{
    public class TrainDTO
    {
        public TrainDTO()
        {
            PassengerClasses = new List<PassengerClass>();
        }
        public string Id { get; set; }
        public string TrainName { get; set; }
        public int SeatCapacity { get; set; }
        public AvailableDays AvailableDays { get; set; }
        public List<PassengerClass> PassengerClasses { get; set; }
        public Status Status { get; set; }
    }
}
