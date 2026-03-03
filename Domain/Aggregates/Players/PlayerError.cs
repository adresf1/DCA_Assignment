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
    public static readonly Error EmailCannotBeEmpty = new("EmailCannotBeEmpty", "Email cannot be empty");
        public static readonly Error InvalidEmailDomain = new("InvalidEmailDomain", "Only people with a VIA mail can register");
        public static readonly Error InvalidEmailFormat = new("InvalidEmailFormat", "Email format is invalid. Must be in format: <text>@<domain>.<extension>");
        public static readonly Error InvalidProfilePictureUri = new("InvalidProfilePictureUri", "Profile picture URL has incorrect format");
        public static readonly Error EmailAlreadyRegistered = new("EmailAlreadyRegistered", "Email is already registered");
}