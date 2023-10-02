namespace TRT.Application.DTOs.ScheduleDTOs
{
    public class ScheduleDTO
    {
        public ScheduleDTO()
        {
            SubStationDetails = new List<SubStationDetailDTO>();
        }
        public string Id { get; set; }
        public string TrainId { get; set; }
        public string DepartureStationId { get; set; }
        public string ArrivalStationId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public List<SubStationDetailDTO> SubStationDetails { get; set; }
    }

    public class SubStationDetailDTO
    {
        public string StationId { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}


