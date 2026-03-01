namespace Domain.Common;

// Strong-typed IDs

public record TimeSlot(DateTime StartTime, DateTime EndTime)
{
    // Business rule: Max 3 hours
    public bool IsValid => EndTime > StartTime && (EndTime - StartTime).TotalHours <= 3;

    // Used to check if two reservations overlap
    public bool Overlaps(TimeSlot other) => 
        StartTime < other.EndTime && other.StartTime < EndTime;
}