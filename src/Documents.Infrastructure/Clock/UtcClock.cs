using Documents.Application.Abstractions;
using Documents.Core.Common.ValueObjects;

namespace Documents.Infrastructure.Clock;

public sealed class UtcClock : IClock
{
    public UtcDateTime Now() => UtcDateTime.Now();
}