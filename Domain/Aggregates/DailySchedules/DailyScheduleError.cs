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
    
    // Use Case 2 errors:
    public static readonly Error CannotModifyActiveSchedule = new("DailySchedule.CannotModifyActive", "An active daily schedule cannot be modified, only deleted");
    public static readonly Error DateCannotBeInPast = new("DailySchedule.DateInPast", "A daily schedule cannot be created in the past");
    public static readonly Error EndTimeBeforeStartTime = new("DailySchedule.EndTimeBeforeStart", "The end time must be after the start time");
    public static readonly Error TimeIntervalTooShort = new("DailySchedule.TimeIntervalTooShort", "The time interval must span 60 minutes or more");
    public static readonly Error InvalidMinutes = new("DailySchedule.InvalidMinutes", "The minutes of the times must be half or whole hours (:00 or :30)");
    
    //USe Case 3 errors:
        public static Error CannotModifyDeletedSchedule => 
            new("DailySchedule.CannotModifyDeleted", "Deleted schedules cannot be updated");
        
        public static Error CannotModifyPastSchedule => 
            new("DailySchedule.CannotModifyPast", "Past schedules cannot be updated");
        
        public static Error InvalidCourtNameLength => 
            new("DailySchedule.InvalidCourtNameLength", "Court name must be 2 or 3 characters long");
        
        public static Error InvalidCourtNameStartingLetter => 
            new("DailySchedule.InvalidCourtNameStartingLetter", "Court name must start with 'S' or 'D'");
        
        public static Error InvalidCourtNameNumber => 
            new("DailySchedule.InvalidCourtNameNumber", "Court name must end with a number between 1 and 10");
        public static readonly Error CourtAlreadyExists = new("COURT_ALREADY_EXISTS", "Court already exists in this daily schedule.");

    
    
}

