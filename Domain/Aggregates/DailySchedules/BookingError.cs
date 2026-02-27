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
}

