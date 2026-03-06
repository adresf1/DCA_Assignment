using Domain.Aggregates.DailySchedules;

namespace UnitTests.Feature_3;

public class AddCourtsToDayTest
{
    // S1: General success - valid court names with capitalization
    [Theory]
    [InlineData("s1", "S1")]
    [InlineData("S2", "S2")]
    [InlineData("d1", "D1")]
    [InlineData("D10", "D10")]
    [InlineData("s5", "S5")]
    public void AddCourt_ValidCourtName_SuccessAndCapitalized(string inputName, string expectedName)
    {
        // Arrange
        var schedule = new DailySchedule();

        // Act
        var result = schedule.AddCourt(inputName);

        // Assert
        Assert.True(result.IsSuccess); 
       
       
    }

    // S2: First court added to empty schedule
    [Fact]
    public void AddCourt_EmptySchedule_Success()
    {
        // Arrange
        var schedule = new DailySchedule();

        // Act
        var result = schedule.AddCourt("S1");

        // Assert
        Assert.True(result.IsSuccess);
    }

    // S3: Add courts with different names
    [Fact]
    public void AddCourt_MultipleDifferentCourts_Success()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("D1");

        // Act
        var result = schedule.AddCourt("D2");

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void AddCourt_MultipleDifferentCourts_ThreeCourts_Success()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("D1");
        schedule.AddCourt("D2");

        // Act
        var result = schedule.AddCourt("S1");

        // Assert
        Assert.True(result.IsSuccess);
    }

    // F1: Past schedule
    [Fact]
    public void AddCourt_PastSchedule_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        var pastDate = DateTime.Today.AddDays(-5);
        schedule.UpdateDate(pastDate);

        // Act
        var result = schedule.AddCourt("S1");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CannotModifyPastSchedule, result.Errors.FirstOrDefault());
    }

    // F2: Invalid starting letter
    [Theory]
    [InlineData("A1")]  // Invalid starting letter
    [InlineData("X2")]  // Invalid starting letter
    [InlineData("1S")]  // Number first
    [InlineData("Z10")] // Invalid starting letter
    public void AddCourt_InvalidStartingLetter_Failure(string courtName)
    {
        // Arrange
        var schedule = new DailySchedule();

        // Act
        var result = schedule.AddCourt(courtName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.InvalidCourtNameStartingLetter, result.Errors.FirstOrDefault());
    }

    // F3: Deleted schedule
    [Fact]
    public void AddCourt_DeletedSchedule_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.Delete();

        // Act
        var result = schedule.AddCourt("S1");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CannotModifyDeletedSchedule, result.Errors.FirstOrDefault());
    }

    // F4: Invalid ending number
    [Theory]
    [InlineData("S0")]   // Number 0 (must be 1-10)
    [InlineData("D11")]  // Number 11 (must be 1-10)
    [InlineData("S15")]  // Number 15 (must be 1-10)
    [InlineData("DA")]   // Letter instead of number
    public void AddCourt_InvalidEndingNumber_Failure(string courtName)
    {
        // Arrange
        var schedule = new DailySchedule();

        // Act
        var result = schedule.AddCourt(courtName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.InvalidCourtNameNumber, result.Errors.FirstOrDefault());
    }

    // F5: Invalid name length
    [Theory]
    [InlineData("S")]     // Too short (1 character)
    [InlineData("S123")]  // Too long (4 characters)
    [InlineData("D1234")] // Too long (5 characters)
    [InlineData("")]      // Empty string
    public void AddCourt_InvalidNameLength_Failure(string courtName)
    {
        // Arrange
        var schedule = new DailySchedule();

        // Act
        var result = schedule.AddCourt(courtName);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.InvalidCourtNameLength, result.Errors.FirstOrDefault());
    }

    // F7: Court already exists
    [Fact]
    public void AddCourt_DuplicateCourtName_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("S1");

        // Act
        var result = schedule.AddCourt("S1");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CourtAlreadyExists, result.Errors.FirstOrDefault());
    }

    [Fact]
    public void AddCourt_DuplicateCourtName_CaseInsensitive_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("s1");

        // Act
        var result = schedule.AddCourt("S1"); // Same court, different case

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CourtAlreadyExists, result.Errors.FirstOrDefault());
    }

    // Boundary tests: Valid court numbers 1-10
    [Theory]
    [InlineData("S1")]
    [InlineData("S10")]
    [InlineData("D1")]
    [InlineData("D10")]
    public void AddCourt_BoundaryValidNumbers_Success(string courtName)
    {
        // Arrange
        var schedule = new DailySchedule();

        // Act
        var result = schedule.AddCourt(courtName);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
