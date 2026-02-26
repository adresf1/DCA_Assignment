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
        EndTime = new TimeSpan(22, 0, 0); // 22:00
        Status = ScheduleStatus.Draft;
    }

    // Use Case 2: Manager updates time and date on a daily schedule

    public Result UpdateDate(DateTime newDate)
    {
        // F5 - Schedule is active
        if (Status == ScheduleStatus.Active)
            return Result.Failure(DailyScheduleError.CannotModifyActiveSchedule);

        // F2 - Date in the past (postponed - requires Domain Contract)
        // if (newDate < DateTime.Today)
        //     return Result.Failure(DailyScheduleError.DateCannotBeInPast);

        Date = newDate;
        return Result.Success();
    }

    
    
    
    public Result UpdateTimeInterval(TimeSpan startTime, TimeSpan endTime)
    {
        // F5 - Schedule is active
        if (Status == ScheduleStatus.Active)
            return Result.Failure(DailyScheduleError.CannotModifyActiveSchedule);

        // F6 - Incorrect minutes (must be :00 or :30)
        if (!IsValidMinutes(startTime) || !IsValidMinutes(endTime))
            return Result.Failure(DailyScheduleError.InvalidMinutes);

        // F3 - End time must be after start time
        if (endTime <= startTime)
            return Result.Failure(DailyScheduleError.EndTimeBeforeStartTime);

        // F4 - Time interval must be 60 minutes or more
        var duration = endTime - startTime;
        if (duration.TotalMinutes < 60)
            return Result.Failure(DailyScheduleError.TimeIntervalTooShort);

        StartTime = startTime;
        EndTime = endTime;
        return Result.Success();
    }

    private static bool IsValidMinutes(TimeSpan time)
    {
        return time.Minutes == 0 || time.Minutes == 30;
    }

    public Result AddCourt(string courtName)
    {
        // F3 - Deleted schedule
        if (Status == ScheduleStatus.Deleted)
            return Result.Failure(DailyScheduleError.CannotModifyDeletedSchedule);

        // F1 - Past schedule
        if (Date < DateTime.Today)
            return Result.Failure(DailyScheduleError.CannotModifyPastSchedule);

        // Validate court name format
        var validationResult = ValidateCourtName(courtName);
        if (validationResult.IsFailure)
            return validationResult;

        // Capitalize court name (S1)
        var normalizedName = courtName.ToUpper();


        // F7 - Court already exists
        if (_courts.Any(c => c.Name == normalizedName))
            return Result.Failure(DailyScheduleError.CourtAlreadyExists);

        var court = new Court(new CourtId(Guid.NewGuid()), normalizedName);
        _courts.Add(court);
        return Result.Success();
    }

    //validate courtName
    public Result ValidateCourtName(string courtName)
    {
        if (courtName.Length < 2 || courtName.Length > 3)
            return Result.Failure(DailyScheduleError.InvalidCourtNameLength);
        if (!courtName.ToUpper().StartsWith("S") && !courtName.ToUpper().StartsWith("D"))
            return Result.Failure(DailyScheduleError.InvalidCourtNameStartingLetter);
        //must be 1-10
        var numberPart = courtName.Substring(1);
        if (!int.TryParse(numberPart, out int courtNumber) || courtNumber < 1 || courtNumber > 10)
            return Result.Failure(DailyScheduleError.InvalidCourtNameNumber);

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