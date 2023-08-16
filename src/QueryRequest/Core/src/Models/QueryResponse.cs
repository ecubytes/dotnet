namespace Ecubytes.QueryModels;

public abstract class QueryResponseBase
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public int Total { get; set; }
}

public class QueryResponse<T> : QueryResponseBase where T : class
{
    public IEnumerable<T>? Data { get; set; }
}

public class QueryResponse : QueryResponse<object>
{
    public static QueryResponse<T> Convert<T>(QueryResponseBase baseModel, IEnumerable<T> data) where T : class
    {
        var response = new QueryResponse<T>
        {
            Page = baseModel.Page,
            Total = baseModel.Total,
            Data = data
        };

        return response;
    }
}