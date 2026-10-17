namespace EventEase.Components.Services;

public sealed record EventAttendanceRecord(
    string EventSlug,
    string EventName,
    DateTime EventDate,
    string Location,
    string AttendeeName,
    string AttendeeEmail,
    DateTimeOffset RegisteredAt,
    DateTimeOffset? CheckedInAt);
