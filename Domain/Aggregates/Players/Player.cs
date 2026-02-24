using Domain.Common;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.Players;

public class Player
{
    public PlayerId ViaId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public bool IsVip { get; private set; }
    public bool IsBlocked { get; private set; }
    //quarantines
    private readonly List<Quarantine> _quarantines = new();
    
    private Player(PlayerId viaId, string name, string email)
    {
        ViaId = viaId;
        Name = name;
        Email = email;
    }
    
    public static Result<Player> CreatePlayer(PlayerId viaId, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            return Result<Player>.Failure(PlayerError.InvalidPlayerData);
        
        if (!email.Contains("@"))
            return Result<Player>.Failure(PlayerError.InvalidPlayerData);
        
        var player = new Player(viaId, name, email);
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
    
    //add quarantine days
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