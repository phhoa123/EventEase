namespace EventEase.Components.Models;

public sealed record EventItem(
    string Slug,
    string Name,
    string Category,
    string ArtTitle,
    DateTime Date,
    string Location,
    string Venue,
    string Address,
    string ArtClass,
    string ArtMark,
    string Description,
    string Time,
    string Audience,
    string Capacity,
    string[] Highlights);

public static class EventCatalog
{
    public static IReadOnlyList<EventItem> All { get; } =
    [
        new(
            "technology-innovation-conference",
            "Technology & Innovation Conference",
            "TECHNOLOGY",
            "Future of Tech",
            new DateTime(2026, 11, 5),
            "National Convention Center, Hanoi",
            "National Convention Center",
            "Phạm Hùng Street, Nam Từ Liêm, Hanoi",
            "art-blue",
            "01",
            "Explore the latest technology trends with leading experts and creators. Hear fresh perspectives on artificial intelligence, sustainable innovation, and the technologies shaping tomorrow.",
            "09:00 — 17:00",
            "Technology professionals, founders, and curious minds",
            "Limited seating",
            ["Keynotes from industry leaders", "Hands-on innovation sessions", "Networking with the tech community"]),
        new(
            "city-music-night",
            "City Music Night",
            "MUSIC",
            "City lights",
            new DateTime(2026, 12, 12),
            "Yen So Park, Hanoi",
            "Yen So Park",
            "Gamuda Central, Hoàng Mai, Hanoi",
            "art-orange",
            "02",
            "Spend an evening under the city lights with live music, great food, and performances from local artists. Bring your friends and enjoy a vibrant outdoor festival.",
            "15:00 — 22:00",
            "Music fans and friends looking for a great night out",
            "Open-air event",
            ["Live performances by local artists", "Food and drink vendors", "Outdoor festival atmosphere"]),
        new(
            "creative-design-workshop",
            "Creative Design Workshop",
            "CREATIVE",
            "Make it matter",
            new DateTime(2027, 1, 18),
            "The Hive, District 1, Ho Chi Minh City",
            "The Hive",
            "26/1 Lê Thánh Tôn, District 1, Ho Chi Minh City",
            "art-lilac",
            "03",
            "Create, share ideas, and turn inspiration into reality alongside fellow designers. This practical workshop takes you from the first sketch to a stronger creative concept.",
            "13:30 — 17:30",
            "Designers, students, and creative professionals",
            "Small-group workshop",
            ["Guided creative exercises", "Feedback from experienced designers", "Take-home project resources"]),
        new(
            "vietnam-startup-festival",
            "Vietnam Startup Festival",
            "NETWORKING",
            "Build what's next",
            new DateTime(2027, 2, 9),
            "White Palace, Ho Chi Minh City",
            "White Palace",
            "194 Hoàng Văn Thụ, Phú Nhuận, Ho Chi Minh City",
            "art-green",
            "04",
            "Meet the startup community, investors, and the people building what comes next. Discover new ideas, hear founder stories, and make connections that move your work forward.",
            "08:30 — 18:00",
            "Founders, investors, and aspiring entrepreneurs",
            "All-day event",
            ["Founder and investor talks", "Startup showcase", "Community networking sessions"])
    ];

    public static EventItem? Find(string? slug) =>
        All.FirstOrDefault(eventItem =>
            string.Equals(eventItem.Slug, slug, StringComparison.OrdinalIgnoreCase));
}
