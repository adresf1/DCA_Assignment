using Domain.Common;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.Players;

public class Player
{
    public PlayerId ViaId { get; private set; }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public Uri ProfilePictureUri { get; private set; }
    public bool IsVip { get; private set; }
    public bool IsBlocked { get; private set; }
    
    private readonly List<Quarantine> _quarantines = new();

    private Player(PlayerId viaId, Common.FirstName firstName, Common.LastName lastName, Email email, Uri profilePictureUri)
    {
        ViaId = viaId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ProfilePictureUri = profilePictureUri;
    }

    // ID: 5 – Register new Player account
    public static Result<Player> Register(string email, string firstName, string lastName, string profilePictureUri)
    {
        // Validate and create Email value object
        var emailResult = Common.Email.Create(email);
        if (emailResult.IsFailure)
            return Result<Player>.Failure(emailResult.Errors.ToArray());

        // Validate and create FirstName value object
        var firstNameResult = Common.FirstName.Create(firstName);
        if (firstNameResult.IsFailure)
            return Result<Player>.Failure(firstNameResult.Errors.ToArray());

        // Validate and create LastName value object
        var lastNameResult = Common.LastName.Create(lastName);
        if (lastNameResult.IsFailure)
            return Result<Player>.Failure(lastNameResult.Errors.ToArray());

        // F4 - Invalid image URI
        if (string.IsNullOrWhiteSpace(profilePictureUri))
            return Result<Player>.Failure(PlayerError.InvalidProfilePictureUri);

        if (!Uri.TryCreate(profilePictureUri, UriKind.Absolute, out Uri? uri))
            return Result<Player>.Failure(PlayerError.InvalidProfilePictureUri);

        var player = new Player(
            new PlayerId(Guid.NewGuid()),
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value,
            uri
        );

        return Result<Player>.Success(player);
    }


    public void ToggleBlock() => IsBlocked = !IsBlocked;

    public void BlackList() => IsBlocked = true;

    public void UnBlackList() => IsBlocked = false;

    public void AddVipStatus() => IsVip = true;

    public bool IsInQuarantine()
    {
        return _quarantines.Any(q => q.StartTime <= DateTime.Now && q.EndTime >= DateTime.Now);
    }

    public Result AddQuarantine(int days, string reason)
    {
        if (days <= 0)
            return Result.Failure(PlayerError.InvalidQuarantineDays);

        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure(PlayerError.InvalidQuarantineReason);

        var startTime = DateTime.Now;
        var endTime = startTime.AddDays(days);
        var quarantine = new Quarantine(startTime, endTime, reason);
        _quarantines.Add(quarantine);

        return Result.Success();
    }
}
