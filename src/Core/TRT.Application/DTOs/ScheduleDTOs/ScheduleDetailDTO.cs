namespace TRT.Application.DTOs.ScheduleDTOs
{
    public class ScheduleDetailDTO
    {
        public string Id { get; set; }
        public string TrainName { get; set; }
        public string DepartureStationName { get; set; }
        public string ArrivalStationName { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
