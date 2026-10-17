using EventEase.Components.Models;

namespace EventEase.Components.Services;

public sealed class UserSessionTracker
{
    private readonly HashSet<string> _registeredEventSlugs = new(StringComparer.OrdinalIgnoreCase);

    public string? AttendeeName { get; private set; }

    public int RegistrationCount => _registeredEventSlugs.Count;

    public bool HasRegistration(string? eventSlug) =>
        !string.IsNullOrWhiteSpace(eventSlug) && _registeredEventSlugs.Contains(eventSlug);

    public void RegisterForEvent(EventItem eventItem, string fullName)
    {
        ArgumentNullException.ThrowIfNull(eventItem);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

        AttendeeName = fullName.Trim();
        _registeredEventSlugs.Add(eventItem.Slug);
    }
}
