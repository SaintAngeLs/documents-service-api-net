using Documents.Core.Abstractions;

namespace Documents.Core.Documents.Exceptions;

public sealed class InvalidTaxRateException : DomainException
{
    public override string Code => "invalid_tax_rate";

    public InvalidTaxRateException(decimal taxRate)
        : base($"Tax rate '{taxRate}' is invalid.")
    {
    }
}