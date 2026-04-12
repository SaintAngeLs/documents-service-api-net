namespace Documents.Application.Documents.DTO;

public sealed class DocumentsAggregateDto
{
    public DateTime GeneratedAtUtc { get; set; }
    public List<DocumentsAggregateRowDto> Rows { get; set; } = [];
}