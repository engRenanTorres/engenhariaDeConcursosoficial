using Microsoft.EntityFrameworkCore;

namespace Application.Core.PagedList;

public class PagedList<T> : List<T>
{
  public int CurrentPage { get; set; }
  public int TotalPages { get; set; }
  public int PageSize { get; set; }
  public int TotalCount { get; set; }

  public PagedList(IEnumerable<T> items, int currentPageNumber, int pageSize, int totalCount)
  {
    CurrentPage = currentPageNumber;
    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    PageSize = pageSize;
    TotalCount = totalCount;
    AddRange(items);
  }

  public static async Task<PagedList<T?>> CreateAsync(
    IQueryable<T?> source,
    int pageNumber,
    int pageSize
  )
  {
    var count = await source.CountAsync();
    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    return new PagedList<T?>(items, pageNumber, pageSize, count);
  }
}
