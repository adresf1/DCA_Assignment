using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.Players;

public class PlayerError
{
    public static readonly Error PlayerNotFound = new("PLAYER_NOT_FOUND", "Player not found.");
    public static readonly Error InvalidPlayerData = new("INVALID_PLAYER_DATA", "Invalid player data.");
    public static readonly Error PlayerAlreadyExists = new("PLAYER_ALREADY_EXISTS", "Player already exists.");
    public static readonly Error PlayerInQuarantine = new("PLAYER_IN_QUARANTINE", "Player is currently in quarantine.");
    public static readonly Error PlayerBlocked = new("PLAYER_BLOCKED", "Player is blocked and cannot perform this action.");
    public static readonly Error InvalidQuarantineDays = new("INVALID_QUARANTINE_DAYS", "Quarantine days must be greater than zero.");
    public static readonly Error InvalidQuarantineReason = new("INVALID_QUARANTINE_REASON", "Quarantine reason cannot be empty.");
}