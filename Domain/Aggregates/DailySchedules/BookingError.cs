using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.Bookings;

public class BookingError
{
    public static readonly Error BookingNotFound = new("BOOKING_NOT_FOUND", "Booking not found.");
    public static readonly Error InvalidTimeSlot = new("INVALID_TIME_SLOT", "Invalid time slot. Must be between 0 and 3 hours.");
    public static readonly Error BookingAlreadyCancelled = new("BOOKING_ALREADY_CANCELLED", "Booking is already cancelled.");
    public static readonly Error BookingNotActive = new("BOOKING_NOT_ACTIVE", "Booking is not active.");
    public static readonly Error CannotAddPlayersToInactiveBooking = new("CANNOT_ADD_PLAYERS", "Cannot add players to a non-active booking.");
    public static readonly Error CannotRemovePlayersFromInactiveBooking = new("CANNOT_REMOVE_PLAYERS", "Cannot remove players from a non-active booking.");
    public static readonly Error CannotMarkInactiveBookingAsNoShow = new("CANNOT_MARK_NO_SHOW", "Cannot mark a non-active booking as no-show.");
    public static readonly Error PlayerNotInBooking = new("PLAYER_NOT_IN_BOOKING", "Player is not in this booking.");
    public static readonly Error ScheduleNotActive = new("SCHEDULE_NOT_ACTIVE", "Courts cannot be booked if the Daily Schedule is not active");
    public static readonly Error BookingStartBeforeScheduleStart = new("BOOKING_START_BEFORE_SCHEDULE", "Booking start time cannot be before schedule start time");
    public static readonly Error BookingEndBeforeScheduleStart = new("BOOKING_END_BEFORE_SCHEDULE", "Booking end time cannot be before schedule start time");
    public static readonly Error BookingStartAfterScheduleEnd = new("BOOKING_START_AFTER_SCHEDULE", "Booking start time cannot be after schedule end time");
    public static readonly Error BookingEndAfterScheduleEnd = new("BOOKING_END_AFTER_SCHEDULE", "Booking end time cannot be after schedule end time");
    public static readonly Error InvalidTimeFormat = new("INVALID_TIME_FORMAT", "Booking times must be on whole or half hours (e.g., 14:00 or 14:30)");
    public static readonly Error BookingTooShort = new("BOOKING_TOO_SHORT", "A booking must be at least one hour");
    public static readonly Error BookingTooLong = new("BOOKING_TOO_LONG", "A booking cannot exceed three hours");
    public static readonly Error BookingOverlapsExisting = new("BOOKING_OVERLAPS", "The court is not available in the selected time span");
    public static readonly Error BookingLeavesSmallGap = new("BOOKING_SMALL_GAP", "A booking may not leave gaps that are less than one hour");
    public static readonly Error BookingCannotStartInPast = new("BOOKING_IN_PAST", "A booking cannot start in the past");
    public static readonly Error BookingInPast = new("BOOKING_IN_PAST_CANCEL", "Past bookings cannot be cancelled");
    public static readonly Error CancellationTooLate = new("CANCELLATION_TOO_LATE", "Booking cannot be cancelled less than one hour before the start time");
    public static readonly Error ScheduleNotFound = new("SCHEDULE_NOT_FOUND", "No schedule found on the requested date");
}

