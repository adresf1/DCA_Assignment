using Domain.Common;

namespace Domain.Aggregates.DailySchedules;

public class Queue
{
    public PlayerId PlayerId { get; private set; }
    public PositionId Position { get; private set; }
    public DateTime RequestedTime { get; private set; }

    public Queue(PlayerId playerId, PositionId position, DateTime requestedTime)
    {
        PlayerId = playerId;
        Position = position;
        RequestedTime = requestedTime;
    }
}