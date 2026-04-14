namespace Documents.Core.Abstractions;

public abstract class DomainException : Exception
{
    public virtual string Code { get; }

    protected DomainException(string message) : base(message)
    {
    }
}