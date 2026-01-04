namespace MRCD.Application.Common;

public sealed record Pagination<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int Size
)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)Size);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public static Pagination<T> Create(IReadOnlyList<T> items, int total, int page, int size)
        => new(items, total, page, size);

};