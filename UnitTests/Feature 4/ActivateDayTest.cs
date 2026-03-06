using Domain.Aggregates.DailySchedules;
using Domain.Common;

namespace UnitTests.Feature_4;

public class ActivateDayTest
{
    // S1 - General success
    [Fact]
    public void ActivateDay_ValidDraftSchedule_Success()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("S1");
        schedule.UpdateDate(DateTime.Today.AddDays(1));

        // Act
        var result = schedule.Activate();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ScheduleStatus.Active, schedule.Status);
    }

    // F1 - Missing courts
    [Fact]
    public void ActivateDay_NoCourts_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.UpdateDate(DateTime.Today.AddDays(1));

        // Act
        var result = schedule.Activate();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CannotActivateWithoutCourts, result.Errors.FirstOrDefault());
    }

    // F2 - Schedule is in the past
    [Fact]
    public void ActivateDay_PastDate_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("S1");
        schedule.UpdateDate(DateTime.Today.AddDays(-1));

        // Act
        var result = schedule.Activate();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.DateCannotBeInPast, result.Errors.FirstOrDefault());
    }

    // F3 - Schedule is deleted
    [Fact]
    public void ActivateDay_DeletedSchedule_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("S1");
        schedule.UpdateDate(DateTime.Today.AddDays(1));
        schedule.Delete();

        // Act
        var result = schedule.Activate();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CannotActivateDeletedSchedule, result.Errors.FirstOrDefault());
    }

    // F6 - Schedule is already active
    [Fact]
    public void ActivateDay_AlreadyActive_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("S1");
        schedule.UpdateDate(DateTime.Today.AddDays(1));
        schedule.Activate();

        // Act
        var result = schedule.Activate();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.DailyScheduleAlreadyActive, result.Errors.FirstOrDefault());
    }


}
