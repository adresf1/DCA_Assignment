using Domain.Aggregates.Players;
using Xunit;

namespace UnitTests.Feature_5;

public class RegisterNewPlayerTest
{
    // S1 - Success scenarios
    [Theory]
    [InlineData("trmo@via.dk", "john", "doe", "https://example.com/pic.jpg", "trmo@via.dk", "John", "Doe")]
    [InlineData("JKNR@VIA.DK", "MARY", "SMITH", "https://example.com/pic.jpg", "jknr@via.dk", "Mary", "Smith")]
    [InlineData("iha@via.dk", "bob", "johnson", "https://example.com/pic.jpg", "iha@via.dk", "Bob", "Johnson")]
    [InlineData("123456@via.dk", "alice", "williams", "https://example.com/pic.jpg", "123456@via.dk", "Alice", "Williams")]
    public void Register_WithValidInputs_CreatesPlayerWithNormalizedValues(
        string email, string firstName, string lastName, string profileUri,
        string expectedEmail, string expectedFirstName, string expectedLastName)
    {
        // Act
        var result = Player.Register(email, firstName, lastName, profileUri);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedEmail, result.Value.Email.Value);
        Assert.Equal(expectedFirstName, result.Value.FirstName.Value);
        Assert.Equal(expectedLastName, result.Value.LastName.Value);
        Assert.NotNull(result.Value.ProfilePictureUri);
        Assert.NotNull(result.Value.ViaId);
    }

    // F1 - Incorrect email domain
    [Theory]
    [InlineData("trmo@gmail.com")]
    [InlineData("trmo@viauc.dk")]
    [InlineData("trmo@via.com")]
    public void Register_WithInvalidEmailDomain_ReturnsFailure(string email)
    {
        // Act
        var result = Player.Register(email, "john", "doe", "https://example.com/pic.jpg");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(PlayerError.InvalidEmailDomain, result.Errors);
    }

    // F2 - Incorrect email format
    [Theory]
    [InlineData("@via.dk")]
    [InlineData("trmo@via")]
    [InlineData("trmo@viadk")]
    [InlineData("trmo@.dk")]
    [InlineData("trmo.via.dk")]
    [InlineData("trmoviadk")]
    public void Register_WithInvalidEmailFormat_ReturnsFailure(string email)
    {
        // Act
        var result = Player.Register(email, "john", "doe", "https://example.com/pic.jpg");

        // Assert
        Assert.True(result.Errors.Any(e => 
            e == PlayerError.InvalidEmailFormat || e == PlayerError.InvalidEmailDomain));
    }

    // F3 - Email is empty
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Register_WithEmptyEmail_ReturnsFailure(string email)
    {
        // Act
        var result = Player.Register(email, "john", "doe", "https://example.com/pic.jpg");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(PlayerError.EmailCannotBeEmpty, result.Errors);
    }

    // F4 - Invalid image URI
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Register_WithInvalidProfilePictureUri_ReturnsFailure(string profileUri)
    {
        // Act
        var result = Player.Register("trmo@via.dk", "john", "doe", profileUri);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(PlayerError.InvalidProfilePictureUri, result.Errors);
    }

    // F5 - First name is invalid
    [Theory]
    [InlineData("a")] // Too short
    [InlineData("abcdefghijklmnopqrstuvwxyz")] // Too long
    [InlineData("John123")] // Contains numbers
    [InlineData("John Doe")] // Contains space
    [InlineData("John-Paul")] // Contains symbol
    [InlineData("")] // Empty
    public void Register_WithInvalidFirstName_ReturnsFailure(string firstName)
    {
        // Act
        var result = Player.Register("trmo@via.dk", firstName, "doe", "https://example.com/pic.jpg");

        // Assert
        Assert.False(result.IsSuccess);
    }

    // F6 - Last name is invalid
    [Theory]
    [InlineData("a")] // Too short
    [InlineData("abcdefghijklmnopqrstuvwxyz")] // Too long
    [InlineData("Doe123")] // Contains numbers
    [InlineData("Van Der Berg")] // Contains spaces
    [InlineData("O'Connor")] // Contains symbol
    [InlineData("")] // Empty
    public void Register_WithInvalidLastName_ReturnsFailure(string lastName)
    {
        // Act
        var result = Player.Register("trmo@via.dk", "john", lastName, "https://example.com/pic.jpg");

        // Assert
        Assert.False(result.IsSuccess);
    }
}
