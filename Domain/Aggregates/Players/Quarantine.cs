﻿namespace Domain.Aggregates.Players;
public class Quarantine
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Reason { get; private set; }

    public Quarantine(DateTime start, DateTime end, string reason)
    {
        StartTime = start;
        EndTime = end;
        Reason = reason;
    }

    public void AddQuarantine(int days)
    {
        EndTime = EndTime.AddDays(days);
    }
    
    
}