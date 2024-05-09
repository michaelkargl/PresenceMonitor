public record PresenceMessageV1(
    Guid id,
    DateTimeOffset presenceMeasuredAt,
    byte presenceCount
);