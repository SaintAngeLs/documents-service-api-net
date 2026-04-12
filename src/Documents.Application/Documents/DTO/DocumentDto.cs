namespace Documents.Application.Documents.DTO;

public sealed class DocumentDto
{
    public Guid Id { get; set; }
    public string DescriptiveNo { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? IntegratedAtUtc { get; set; }
    public int Revision { get; set; }
}