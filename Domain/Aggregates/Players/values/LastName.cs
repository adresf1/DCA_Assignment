using System.Text.RegularExpressions;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Common;

public record LastName
{
    public string Value { get; }

    private LastName(string value)
    {
        Value = value;
    }

    public static Result<LastName> Create(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            return Result<LastName>.Failure(new Error("InvalidLastName", "Last name cannot be empty"));

        var trimmedName = lastName.Trim();

        // Must be between 2 and 25 letters
        if (trimmedName.Length < 2 || trimmedName.Length > 25)
            return Result<LastName>.Failure(new Error("InvalidLastName", "Last name must be between 2 and 25 letters"));

        // Must contain only letters (a-z)
        if (!Regex.IsMatch(trimmedName, @"^[a-zA-Z]+$"))
            return Result<LastName>.Failure(new Error("InvalidLastName", "Last name must contain only letters"));

        // Capitalize: first letter uppercase, rest lowercase
        var capitalizedName = char.ToUpper(trimmedName[0]) + trimmedName.Substring(1).ToLower();

        return Result<LastName>.Success(new LastName(capitalizedName));
    }

    public override string ToString() => Value;
}

