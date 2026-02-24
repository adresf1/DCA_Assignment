using Domain.Common;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.DailySchedules;



public class DailySchedule
{
    public DailyScheduleId Id { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public ScheduleStatus Status { get; private set; }
    
    private readonly List<Court> _courts = new();
    private readonly List<Queue> _queue = new();
    private readonly List<BookingId> _bookings = new();

    // Use Case 1: Manager creates daily schedule
    // - ID is auto-generated
    // - Status is set to "draft"
    // - List of available courts is empty
    // - Times are set to 15:00 and 22:00
    // - Date is set to today
    public DailySchedule()
    {
        Id = new DailyScheduleId(Guid.NewGuid());
        Date = DateTime.Today;
        StartTime = new TimeSpan(15, 0, 0); // 15:00
        EndTime = new TimeSpan(22, 0, 0);   // 22:00
        Status = ScheduleStatus.Draft;
    }
    
    

    public Result AddCourt(Court court)
    {
        if (_courts.Contains(court))
            return Result.Failure(DailyScheduleError.CourtAlreadyExists);
        
        _courts.Add(court);
        return Result.Success();
    }
    
    public Result RemoveCourt(Court court)
    {
        if (!_courts.Contains(court))
            return Result.Failure(DailyScheduleError.CourtNotFound);
        
        _courts.Remove(court);
        return Result.Success();
    }

    public void AddToQueue(PlayerId playerId)
    {
        _queue.Add(new Queue(playerId, new PositionId(Guid.NewGuid()), DateTime.Now));
    }

    public void RegisterBooking(BookingId bookingId) => _bookings.Add(bookingId);

    public Result Activate()
    {
        if (Status == ScheduleStatus.Deleted)
            return Result.Failure(DailyScheduleError.CannotActivateDeletedSchedule);
        
        if (Status == ScheduleStatus.Active)
            return Result.Failure(DailyScheduleError.DailyScheduleAlreadyActive);
        
        if (!_courts.Any())
            return Result.Failure(DailyScheduleError.CannotActivateWithoutCourts);
        
        Status = ScheduleStatus.Active;
        return Result.Success();
    }

    public Result Delete()
    {
        if (Status == ScheduleStatus.Deleted)
            return Result.Failure(DailyScheduleError.DailyScheduleAlreadyDeleted);
        
        if (_bookings.Any())
            return Result.Failure(DailyScheduleError.CannotDeleteWithBookings);
        
        Status = ScheduleStatus.Deleted;
        _courts.Clear();
        _queue.Clear();
        return Result.Success();
    }
    
        
    }
