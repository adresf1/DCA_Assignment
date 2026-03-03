using System.Text.RegularExpressions;
using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Common;

public record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        // F3 - Email is empty
        if (string.IsNullOrWhiteSpace(email))
            return Result<Email>.Failure(new Error("EmailCannotBeEmpty", "Email cannot be empty"));

        // Normalize email to lowercase
        var normalizedEmail = email.Trim().ToLower();

        // F1 - Incorrect email domain
        if (!normalizedEmail.EndsWith("@via.dk"))
            return Result<Email>.Failure(new Error("InvalidEmailDomain", "Only people with a VIA mail can register"));

        // F2 - Incorrect email format
        var validation = ValidateFormat(normalizedEmail);
        if (validation.IsFailure)
            return Result<Email>.Failure(validation.Errors.ToArray());

        return Result<Email>.Success(new Email(normalizedEmail));
    }

    private static Result ValidateFormat(string email)
    {
        // Format: <text1>@<text2>.<text3>
        var parts = email.Split('@');
        if (parts.Length != 2)
            return Result.Failure(new Error("InvalidEmailFormat", "Email format is invalid. Must be in format: <text>@<domain>.<extension>"));

        var localPart = parts[0];
        var domainPart = parts[1];

        // Check if localPart is 3, 4, or 6 characters
        if (localPart.Length != 3 && localPart.Length != 4 && localPart.Length != 6)
            return Result.Failure(new Error("InvalidEmailFormat", "Email format is invalid. Must be in format: <text>@<domain>.<extension>"));

        // For 3 or 4 characters: must be all letters
        if (localPart.Length == 3 || localPart.Length == 4)
        {
            if (!Regex.IsMatch(localPart, @"^[a-z]{" + localPart.Length + "}$"))
                return Result.Failure(new Error("InvalidEmailFormat", "Email format is invalid. Must be in format: <text>@<domain>.<extension>"));
        }
        // For 6 characters: must be all digits
        else if (localPart.Length == 6)
        {
            if (!Regex.IsMatch(localPart, @"^[0-9]{6}$"))
                return Result.Failure(new Error("InvalidEmailFormat", "Email format is invalid. Must be in format: <text>@<domain>.<extension>"));
        }

        // Check domain has correct format (text2.text3)
        var domainParts = domainPart.Split('.');
        if (domainParts.Length < 2 || string.IsNullOrWhiteSpace(domainParts[0]) || string.IsNullOrWhiteSpace(domainParts[1]))
            return Result.Failure(new Error("InvalidEmailFormat", "Email format is invalid. Must be in format: <text>@<domain>.<extension>"));

        return Result.Success();
    }

    public override string ToString() => Value;
}

