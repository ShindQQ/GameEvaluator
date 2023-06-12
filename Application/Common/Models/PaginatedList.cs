using Microsoft.EntityFrameworkCore;

namespace Application.Common.Models;

public sealed class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < PageSize;

    public PaginatedList(
        IReadOnlyCollection<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, pageNumber, pageSize, count);
    }
}
