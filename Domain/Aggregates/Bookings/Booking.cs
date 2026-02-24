using Domain.Common;
using Domain.Aggregates.Players;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.Bookings;

public class Booking
{
    public BookingId Id { get; private set; }
    public TimeSlot Slot { get; private set; }
    public BookingStatus Status { get; private set; }
    public Player BookedBy { get; private set; }
    public CourtId CourtId { get; private set; }
    private readonly List<Player> _players = new();
    
    private Booking(BookingId id, TimeSlot slot, Player bookedBy, CourtId courtId)
    {
        Id = id;
        Slot = slot;
        BookedBy = bookedBy;
        CourtId = courtId;
        Status = BookingStatus.Active;
    }
    
    //create booking 
    public static Result<Booking> CreateBooking(BookingId id, TimeSlot slot, Player bookedBy, CourtId courtId)
    {
        if (!slot.IsValid)
            return Result<Booking>.Failure(BookingError.InvalidTimeSlot);
        
        if (bookedBy.IsBlocked)
            return Result<Booking>.Failure(PlayerError.PlayerBlocked);
        
        if (bookedBy.IsInQuarantine())
            return Result<Booking>.Failure(PlayerError.PlayerInQuarantine);
        
        var booking = new Booking(id, slot, bookedBy, courtId);
        return Result<Booking>.Success(booking);
    }
    
    public Result CancelBooking()
    {
        if (Status == BookingStatus.Cancelled)
            return Result.Failure(BookingError.BookingAlreadyCancelled);
        
        Status = BookingStatus.Cancelled;
        return Result.Success();
    }
    
    public Result AddPlayersToCourt(List<Player> players)
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
    
    public Result RemovePlayerFromCourt(Player player)
    {
        if (Status != BookingStatus.Active)
            return Result.Failure(BookingError.CannotRemovePlayersFromInactiveBooking);
        
        if (!_players.Contains(player))
            return Result.Failure(BookingError.PlayerNotInBooking);
        
        _players.Remove(player);
        return Result.Success();
    }
    


}