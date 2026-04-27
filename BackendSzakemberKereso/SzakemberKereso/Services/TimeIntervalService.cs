using Microsoft.EntityFrameworkCore;
using SzakemberKereso.DTOs.TimeInterval;
using SzakemberKereso.Models;

namespace SzakemberKereso.Services
{
    public class TimeIntervalService(Context context)
    {
        public bool IsValid(InputTimeIntervalDto interval)
        {
            return interval.StartTime < interval.EndTime; 
        } 

        //checks for collisions between the provided intervals, really inefficient I know
        public bool HasInternalCollisions(IEnumerable<InputTimeIntervalDto> intervals)
        {
            var list = intervals.ToList();
            for (int i = 0; i < list.Count; i++)
                for (int j = i + 1; j < list.Count; j++)
                    if (list[i].Date == list[j].Date &&
                        list[i].StartTime < list[j].EndTime &&
                        list[j].StartTime < list[i].EndTime)
                        return true;
            return false;
        }

        //checks for collisions between the provided intervals and the onces already in the db for the expert (for Offered and Accepted state)
        public async Task<bool> HasExpertCollisionsAsync(int expertId, IEnumerable<InputTimeIntervalDto> proposed)
        {
            var committed = await context.TimeIntervals
                .Where(t =>
                    t.Type == TimeIntervalType.Offered &&
                    (t.Job.Status == JobStatus.Offered || t.Job.Status == JobStatus.Accepted) &&
                    t.Job.Service.ExpertSpecialty.ExpertId == expertId)
                .Select(t => new { t.Date, t.StartTime, t.EndTime })
                .ToListAsync();

            return proposed.Any(p =>
                committed.Any(c =>
                    c.Date == p.Date &&
                    c.StartTime < p.EndTime &&
                    p.StartTime < c.EndTime));
        }
    }
}
