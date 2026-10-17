using EventEase.Components.Models;

namespace EventEase.Components.Services;

public sealed class AttendanceTrackerService
{
    private readonly Dictionary<string, EventAttendanceRecord> _records =
        new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<EventAttendanceRecord> Records =>
        _records.Values
            .OrderBy(record => record.EventDate)
            .ToArray();

    public int RegisteredCount => _records.Count;

    public int AttendedCount => _records.Values.Count(record => record.CheckedInAt is not null);

    public bool HasRegistration(string? eventSlug) =>
        !string.IsNullOrWhiteSpace(eventSlug) && _records.ContainsKey(eventSlug);

    public void Register(EventItem eventItem, string attendeeName, string attendeeEmail)
    {
        ArgumentNullException.ThrowIfNull(eventItem);
        ArgumentException.ThrowIfNullOrWhiteSpace(attendeeName);
        ArgumentException.ThrowIfNullOrWhiteSpace(attendeeEmail);

        var normalizedName = attendeeName.Trim();
        var normalizedEmail = attendeeEmail.Trim();
        var checkedInAt = _records.TryGetValue(eventItem.Slug, out var current)
            ? current.CheckedInAt
            : null;

        _records[eventItem.Slug] = new EventAttendanceRecord(
            eventItem.Slug,
            eventItem.Name,
            eventItem.Date,
            eventItem.Location,
            normalizedName,
            normalizedEmail,
            DateTimeOffset.UtcNow,
            checkedInAt);
    }

    public bool MarkAttended(string? eventSlug)
    {
        if (string.IsNullOrWhiteSpace(eventSlug) ||
            !_records.TryGetValue(eventSlug, out var record))
        {
            return false;
        }

        if (record.CheckedInAt is null)
        {
            _records[eventSlug] = record with { CheckedInAt = DateTimeOffset.UtcNow };
        }

        return true;
    }
}
