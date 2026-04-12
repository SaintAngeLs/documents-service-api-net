using Documents.Core.Abstractions;

namespace Documents.Core.Documents.Exceptions;

public sealed class EmptyDescriptiveNoException : DomainException
{
    public override string Code => "empty_descriptive_no";

    public EmptyDescriptiveNoException()
        : base("Document descriptive number cannot be empty.")
    {
    }
}