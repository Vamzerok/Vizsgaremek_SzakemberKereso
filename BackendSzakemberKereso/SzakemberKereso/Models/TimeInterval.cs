using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SzakemberKereso.Models
{
    public enum TimeIntervalType
    {
        Available,
        Offered
    }

    [Table("time_intervals")]
    public class TimeInterval
    {
        [Key]
        [Column("time_interval_id")]
        public int Id { get; set; }

        [Column("type")]
        public TimeIntervalType Type { get; set; }

        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("start_time")]
        public TimeOnly StartTime { get; set; }

        [Column("end_time")]
        public TimeOnly EndTime { get; set; }

        [Column("job_id")]
        public int JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        public virtual Job Job { get; set; } = null!;
    }
}
