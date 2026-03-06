using Domain.Aggregates.DailySchedules;
using Domain.Common;

namespace UnitTests.Feature_2;

public class UpdateTimeAndDate
{
    // S1: Update date in draft status with future date
    [Fact]
    public void UpdateDate_DraftSchedule_FutureDate_Success()
    {
        // Arrange
        var schedule = new DailySchedule();
        var futureDate = DateTime.Today.AddDays(5);

        // Act
        var result = schedule.UpdateDate(futureDate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(futureDate, schedule.Date);
        Assert.Equal(ScheduleStatus.Draft, schedule.Status);
    }

    // S2: Update time interval with valid times
    [Theory]
    [InlineData(10, 0, 14, 0)]  // 10:00 to 14:00
    [InlineData(8, 0, 16, 0)]   // 08:00 to 16:00
    [InlineData(15, 30, 22, 0)] // 15:30 to 22:00
    public void UpdateTimeInterval_ValidTimes_Success(int startHour, int startMin, int endHour, int endMin)
    {
        // Arrange
        var schedule = new DailySchedule();
        var startTime = new TimeSpan(startHour, startMin, 0);
        var endTime = new TimeSpan(endHour, endMin, 0);

        // Act
        var result = schedule.UpdateTimeInterval(startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, schedule.StartTime);
        Assert.Equal(endTime, schedule.EndTime);
        Assert.Equal(ScheduleStatus.Draft, schedule.Status);
    }
    [Theory]
    [InlineData(10, 15, 14, 0)]  // Invalid start minutes
    [InlineData(10, 0, 14, 45)]  // Invalid end minutes
    [InlineData(10, 23, 14, 17)] // Both invalid
    public void InvalidMinutesTest_failure(int startHour, int startMin, int endHour, int endMin)
    {
        // Arrange
        var schedule = new DailySchedule();
        var startTime = new TimeSpan(startHour, startMin, 0);
        var endTime = new TimeSpan(endHour, endMin, 0);

        // Act
        var result = schedule.UpdateTimeInterval(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.InvalidMinutes, result.Errors.FirstOrDefault());
    }

    // F3: End time before start time
    // F3: End time before start time
    [Theory]
    [InlineData(10, 0, 9, 0)]   // 10:00 to 09:00
    [InlineData(8, 0, 7, 30)]   // 08:00 to 07:30 (changed from 07:59)
    [InlineData(12, 0, 12, 0)]  // 12:00 to 12:00
    public void UpdateTimeInterval_EndTimeBeforeStartTime_Failure(int startHour, int startMin, int endHour, int endMin)
    {
        // Arrange
        var schedule = new DailySchedule();
        var startTime = new TimeSpan(startHour, startMin, 0);
        var endTime = new TimeSpan(endHour, endMin, 0);

        // Act
        var result = schedule.UpdateTimeInterval(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.EndTimeBeforeStartTime, result.Errors.FirstOrDefault());
    }

    

    // F4: Time interval too short (less than 60 minutes)
    [Theory]
    [InlineData(10, 0, 10, 30)]  // 10:00 to 10:30 (30 minutes)
    [InlineData(8, 30, 9, 0)]    // 08:30 to 09:00 (30 minutes)
    [InlineData(12, 0, 12, 30)]  // 12:00 to 12:30 (30 minutes)
    public void UpdateTimeInterval_TooShort_Failure(int startHour, int startMin, int endHour, int endMin)
    {
        // Arrange
        var schedule = new DailySchedule();
        var startTime = new TimeSpan(startHour, startMin, 0);
        var endTime = new TimeSpan(endHour, endMin, 0);

        // Act
        var result = schedule.UpdateTimeInterval(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.TimeIntervalTooShort, result.Errors.FirstOrDefault());
    }


    // F5: Cannot modify active schedule (date)
    [Fact]
    public void UpdateDate_ActiveSchedule_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("S1");
        schedule.Activate();
        var futureDate = DateTime.Today.AddDays(5);

        // Act
        var result = schedule.UpdateDate(futureDate);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CannotModifyActiveSchedule, result.Errors.FirstOrDefault());
    }

    // F5: Cannot modify active schedule (time)
    [Fact]
    public void UpdateTimeInterval_ActiveSchedule_Failure()
    {
        // Arrange
        var schedule = new DailySchedule();
        schedule.AddCourt("S1");
        schedule.Activate();

        // Act
        var result = schedule.UpdateTimeInterval(new TimeSpan(10, 0, 0), new TimeSpan(14, 0, 0));

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.CannotModifyActiveSchedule, result.Errors.FirstOrDefault());
    }

    // F6: Invalid minutes (must be :00 or :30)
    [Theory]
    [InlineData(10, 15, 14, 0)]  // Invalid start minutes
    [InlineData(10, 0, 14, 45)]  // Invalid end minutes
    [InlineData(10, 23, 14, 17)] // Both invalid
    public void UpdateTimeInterval_InvalidMinutes_Failure(int startHour, int startMin, int endHour, int endMin)
    {
        // Arrange
        var schedule = new DailySchedule();
        var startTime = new TimeSpan(startHour, startMin, 0);
        var endTime = new TimeSpan(endHour, endMin, 0);

        // Act
        var result = schedule.UpdateTimeInterval(startTime, endTime);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DailyScheduleError.InvalidMinutes, result.Errors.FirstOrDefault());
    }
}
