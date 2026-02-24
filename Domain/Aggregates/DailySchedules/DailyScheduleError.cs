using ViaPadel.Core.Tools.OperationResult;

namespace Domain.Aggregates.DailySchedules;

public class DailyScheduleError
{
    public static readonly Error DailyScheduleNotFound = new("DAILY_SCHEDULE_NOT_FOUND", "Daily schedule not found.");
    public static readonly Error DailyScheduleAlreadyActive = new("DAILY_SCHEDULE_ALREADY_ACTIVE", "Daily schedule is already active.");
    public static readonly Error DailyScheduleAlreadyDeleted = new("DAILY_SCHEDULE_ALREADY_DELETED", "Daily schedule is already deleted.");
    public static readonly Error CannotActivateDeletedSchedule = new("CANNOT_ACTIVATE_DELETED", "Cannot activate a deleted daily schedule.");
    public static readonly Error CannotActivateWithoutCourts = new("CANNOT_ACTIVATE_WITHOUT_COURTS", "Cannot activate a daily schedule without courts.");
    public static readonly Error CannotDeleteWithBookings = new("CANNOT_DELETE_WITH_BOOKINGS", "Cannot delete a daily schedule with existing bookings.");
    public static readonly Error CourtNotFound = new("COURT_NOT_FOUND", "Court not found in this daily schedule.");
    public static readonly Error CourtAlreadyExists = new("COURT_ALREADY_EXISTS", "Court already exists in this daily schedule.");
}

