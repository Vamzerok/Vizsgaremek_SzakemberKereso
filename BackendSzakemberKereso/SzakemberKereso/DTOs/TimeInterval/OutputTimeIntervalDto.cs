using SzakemberKereso.Models;

namespace SzakemberKereso.DTOs.TimeInterval
{
    public class OutputTimeIntervalDto
    {
        public int Id { get; set; }
        public TimeIntervalType Type { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
