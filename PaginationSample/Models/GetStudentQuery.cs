namespace PaginationSample.Models
{
    public record GetStudentQuery(
        int Page,
        int PageSize,
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder
        );
}
