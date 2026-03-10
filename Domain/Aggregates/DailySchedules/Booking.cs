using Domain.Common;
using Domain.Aggregates.Players;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.Bookings;

public class Booking
{
    public BookingId Id { get; private set; }
    public TimeSlot Slot { get; private set; }
    public BookingStatus Status { get; private set; }
    public PlayerId BookedBy { get; private set; }
    public CourtId CourtId { get; private set; }
    private readonly List<PlayerId> _players = new();
    
    private Booking(BookingId id, TimeSlot slot, PlayerId bookedBy, CourtId courtId)
    {
        Id = id;
        Slot = slot;
        BookedBy = bookedBy;
        CourtId = courtId;
        Status = BookingStatus.Active;
    }

    // CREATE BOOKING - REQUIREMENT

    public static Result<Booking> Create(
        BookingId bookingId,
        TimeSlot slot,
        PlayerId bookedBy,
        CourtId courtId,
        DailySchedule schedule,
        List<Booking> existingBookingsOnCourt)
    {
        if (schedule.Status != ScheduleStatus.Active)
            return Result.Failure(BookingError.ScheduleNotActive);

        var timeValidation = ValidateTimeWithinSchedule(slot, schedule);
        if (!timeValidation.IsSuccess)
            return timeValidation;

        var formatValidation = ValidateTimeFormat(slot);
        if (!formatValidation.IsSuccess)
            return formatValidation;

        var minDurationValidation = ValidateDuration(slot, minHours: 1);
        if (!minDurationValidation.IsSuccess)
            return minDurationValidation;

        var maxDurationValidation = ValidateDuration(slot, maxHours: 3);
        if (!maxDurationValidation.IsSuccess)
            return maxDurationValidation;

        var overlapValidation = ValidateNoOverlap(slot, existingBookingsOnCourt);
        if (!overlapValidation.IsSuccess)
            return overlapValidation;

        var gapValidation = ValidateNoSmallGaps(slot, existingBookingsOnCourt, schedule);
        if (!gapValidation.IsSuccess)
            return gapValidation;

        if (slot.StartTime < DateTime.UtcNow)
            return Result.Failure(BookingError.BookingCannotStartInPast);

        var booking = new Booking(bookingId, slot, bookedBy, courtId);
        return Result.Success(booking);
    }


    // CANCEL BOOKING - REQUIREMENT


    public Result CancelBooking(DateTime cancellationTime)
    {
        if (Status == BookingStatus.Cancelled)
            return Result.Failure(BookingError.BookingAlreadyCancelled);

        if (Slot.StartTime < DateTime.UtcNow)
            return Result.Failure(BookingError.BookingInPast);

        var timeUntilStart = Slot.StartTime - cancellationTime;
        if (timeUntilStart <= TimeSpan.FromHours(1))
            return Result.Failure(BookingError.CancellationTooLate);

        Status = BookingStatus.Cancelled;
        return Result.Success();
    }


    public Result AddPlayersToCourt(List<PlayerId> players)
    {
        if (Status != BookingStatus.Active)
            return Result.Failure(BookingError.CannotAddPlayersToInactiveBooking);

        _players.AddRange(players);
        return Result.Success();
    }

    public Result MarkNoShow()
    {
        if (Status != BookingStatus.Active)
            return Result.Failure(BookingError.CannotMarkInactiveBookingAsNoShow);

        Status = BookingStatus.NoShow;
        return Result.Success();
    }

    public Result RemovePlayerFromCourt(PlayerId player)
    {
        if (Status != BookingStatus.Active)
            return Result.Failure(BookingError.CannotRemovePlayersFromInactiveBooking);

        if (!_players.Contains(player))
            return Result.Failure(BookingError.PlayerNotInBooking);

        _players.Remove(player);
        return Result.Success();
    }


    private static Result ValidateTimeWithinSchedule(TimeSlot slot, DailySchedule schedule)
    {
        if (slot.StartTime < schedule.StartTime)
            return Result.Failure(BookingError.BookingStartBeforeScheduleStart);

        if (slot.EndTime < schedule.StartTime)
            return Result.Failure(BookingError.BookingEndBeforeScheduleStart);

        if (slot.StartTime > schedule.EndTime)
            return Result.Failure(BookingError.BookingStartAfterScheduleEnd);

        if (slot.EndTime > schedule.EndTime)
            return Result.Failure(BookingError.BookingEndAfterScheduleEnd);

        return Result.Success();
    }

    private static Result ValidateTimeFormat(TimeSlot slot)
    {
        if ((slot.StartTime.Minute != 0 && slot.StartTime.Minute != 30) ||
            (slot.EndTime.Minute != 0 && slot.EndTime.Minute != 30))
            return Result.Failure(BookingError.InvalidTimeFormat);

        return Result.Success();
    }

    private static Result ValidateDuration(TimeSlot slot, int? minHours = null, int? maxHours = null)
    {
        var duration = (slot.EndTime - slot.StartTime).TotalHours;

        if (minHours.HasValue && duration < minHours)
            return Result.Failure(BookingError.BookingTooShort);

        if (maxHours.HasValue && duration > maxHours)
            return Result.Failure(BookingError.BookingTooLong);

        return Result.Success();
    }

    private static Result ValidateNoOverlap(TimeSlot slot, List<Booking> existingBookings)
    {
        foreach (var booking in existingBookings.Where(b => b.Status == BookingStatus.Active))
        {
            if (slot.StartTime < booking.Slot.EndTime && slot.EndTime > booking.Slot.StartTime)
                return Result.Failure(BookingError.BookingOverlapsExisting);
        }

        return Result.Success();
    }

    private static Result ValidateNoSmallGaps(TimeSlot slot, List<Booking> existingBookings, DailySchedule schedule)
    {
        var activeBookings = existingBookings
            .Where(b => b.Status == BookingStatus.Active)
            .OrderBy(b => b.Slot.StartTime)
            .ToList();

        var previousBooking = activeBookings.LastOrDefault(b => b.Slot.EndTime <= slot.StartTime);
        if (previousBooking != null)
        {
            var gapBefore = (slot.StartTime - previousBooking.Slot.EndTime).TotalHours;
            if (gapBefore > 0 && gapBefore < 1)
                return Result.Failure(BookingError.BookingLeavesSmallGap);
        }
        else if (slot.StartTime > schedule.StartTime)
        {
            var gapFromStart = (slot.StartTime - schedule.StartTime).TotalHours;
            if (gapFromStart > 0 && gapFromStart < 1)
                return Result.Failure(BookingError.BookingLeavesSmallGap);
        }

        var nextBooking = activeBookings.FirstOrDefault(b => b.Slot.StartTime >= slot.EndTime);
        if (nextBooking != null)
        {
            var gapAfter = (nextBooking.Slot.StartTime - slot.EndTime).TotalHours;
            if (gapAfter > 0 && gapAfter < 1)
                return Result.Failure(BookingError.BookingLeavesSmallGap);
        }
        else if (slot.EndTime < schedule.EndTime)
        {
            var gapToEnd = (schedule.EndTime - slot.EndTime).TotalHours;
            if (gapToEnd > 0 && gapToEnd < 1)
                return Result.Failure(BookingError.BookingLeavesSmallGap);
        }

        return Result.Success();
    }
}