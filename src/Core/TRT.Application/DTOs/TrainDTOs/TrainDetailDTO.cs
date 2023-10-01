using TRT.Domain.Enums;

namespace TRT.Application.DTOs.TrainDTOs
{
    public class TrainDetailDTO
    {
        public string Id { get; set; }
        public string TrainName { get; set; }
        public int SeatCapacity { get; set; }
        public Status Status { get; set; }
        public string TrainAvailableDays { get; set; }
        public string PassengerClassNames { get; set; }
    }
}
