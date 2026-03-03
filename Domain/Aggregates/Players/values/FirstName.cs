using System.Text.RegularExpressions;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Common;

public record FirstName
{
    public string Value { get; }

    private FirstName(string value)
    {
        Value = value;
    }

    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result<FirstName>.Failure(new Error("InvalidFirstName", "First name cannot be empty"));

        var trimmedName = firstName.Trim();

        // Must be between 2 and 25 letters
        if (trimmedName.Length < 2 || trimmedName.Length > 25)
            return Result<FirstName>.Failure(new Error("InvalidFirstName", "First name must be between 2 and 25 letters"));

        // Must contain only letters (a-z)
        if (!Regex.IsMatch(trimmedName, @"^[a-zA-Z]+$"))
            return Result<FirstName>.Failure(new Error("InvalidFirstName", "First name must contain only letters"));

        // Capitalize: first letter uppercase, rest lowercase
        var capitalizedName = char.ToUpper(trimmedName[0]) + trimmedName.Substring(1).ToLower();

        return Result<FirstName>.Success(new FirstName(capitalizedName));
    }

    public override string ToString() => Value;
}

