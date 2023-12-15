namespace GoogleCalender1._0.DTO
{
    public class GoogleCalenderReq
    {
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? CalenderId { get; set; }
        public string? refreshToken { get; set; }
   
    }
} 