namespace Domain.Common;

//playerId
public record PlayerId(Guid Value);
public record BookingId(Guid Value);
public record CourtId(Guid Value);
public record PositionId(Guid Value);
public record DailyScheduleId(Guid Value);

public record TimeSlot(DateTime StartTime, DateTime EndTime)
{
    // Forretningsregel: Max 3 timer
    public bool IsValid => EndTime > StartTime && (EndTime - StartTime).TotalHours <= 3;

    // Bruges til at tjekke om to reservationer overlapper
    public bool Overlaps(TimeSlot other) => 
        StartTime < other.EndTime && other.StartTime < EndTime;
}