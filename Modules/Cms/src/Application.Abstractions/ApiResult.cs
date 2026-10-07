using System.Collections.Generic;
using System.Linq;

namespace FluentCMS;

public interface IApiResult
{
    List<AppError> Errors { get; }
    string TraceId { get; set; }
    string SessionId { get; set; }
    string UniqueId { get; set; }
    double Duration { get; set; }
    int Status { get; set; }
    bool IsSuccess { get; set; }
}
public interface IApiResult<TData> : IApiResult
{
    TData? Data { get; }
}

public class ApiResult<TData> : IApiResult<TData>
{
    public List<AppError> Errors { get; } = [];
    public string TraceId { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public string UniqueId { get; set; } = string.Empty;
    public double Duration { get; set; }
    public int Status { get; set; }
    public TData? Data { get; }
    public bool IsSuccess { get; set; } = true;

    public ApiResult()
    {
    }

    public ApiResult(TData data)
    {
        Data = data;
    }
}

public interface IApiPagingResult<TData> : IApiResult<IEnumerable<TData>>
{
    int PageSize { get; }
    int TotalPages { get; }
    int PageNumber { get; }
    long TotalCount { get; }
    bool HasPrevious { get; }
    bool HasNext { get; }
}

public class ApiPagingResult<TData> : ApiResult<IEnumerable<TData>>, IApiPagingResult<TData>
{
    public int PageSize { get; }
    public int TotalPages { get; }
    public int PageNumber { get; }
    public long TotalCount { get; }
    public bool HasPrevious { get; }
    public bool HasNext { get; }
    public ApiPagingResult(IEnumerable<TData> data) : base(data)
    {
        // Initialize properties based on the data
        PageNumber = 1;
        PageSize = data.Count();
        TotalCount = data.Count();
        HasPrevious = PageNumber > 1;
        HasNext = PageNumber < TotalPages;
    }
}
