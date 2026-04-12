using Documents.Core.Common.ValueObjects;

namespace Documents.Application.Abstractions;

public interface IClock
{
    UtcDateTime Now();
}