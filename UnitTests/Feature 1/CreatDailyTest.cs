using Domain.Aggregates.DailySchedules;
using Domain.Common;

namespace UnitTests.Feature_1;

public class CreatDailyTest
{
    
    [Fact]
    public void CreateDailySchedule()
    {

        DailySchedule dailySchedule = new DailySchedule();
        Assert.NotNull(dailySchedule.Id);
        Assert.Equal(new TimeSpan(15, 0, 0), dailySchedule.StartTime);
        Assert.Equal(new TimeSpan(22, 0, 0), dailySchedule.EndTime);
        Assert.Equal(ScheduleStatus.Draft, dailySchedule.Status);
        Assert.Equal(DateTime.Today, dailySchedule.Date);
    }
        
}