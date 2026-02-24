using Domain.Common;

namespace Domain.Aggregates.DailySchedules;

public class Court
{
    public CourtId Id { get; private set; }
    public CourtType Type { get; private set; }

    public Court(CourtId id, CourtType type)
    {
        Id = id;
        Type = type;
    }
}