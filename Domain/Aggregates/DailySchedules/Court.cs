using Domain.Common;

namespace Domain.Aggregates.DailySchedules;

public class Court
{
    public CourtId Id { get; private set; }
    public string Name { get; private set; }
    public CourtType Type { get; private set; }

    public Court(CourtId id, string name)
    {
        Id = id;
        Name = name;
        // Determine type from name: names starting with 'S' are Single, 'D' are Double
        Type = name.ToUpper().StartsWith("S") ? CourtType.Single : CourtType.Double;
    }
}