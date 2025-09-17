public sealed class TrainingListQuery
{
    public string? Search { get; init; }
    public int? CategoryId { get; init; }
    public bool? IsPublished { get; init; }  
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}