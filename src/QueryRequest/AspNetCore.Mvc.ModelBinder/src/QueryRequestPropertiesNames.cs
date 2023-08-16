namespace Ecubytes.QueryResult.AspNetCore.Mvc.ModelBinder;

internal static class QueryRequestPropertiesNames
{
    public const string Page = "page";
    public const string PageSize = "pageSize";
    public const string SearchValue = "search";
}

internal static class QueryFieldSortPropertiesNames
{
    public const string Entity = "__qsort";
    public const string FieldName = "__n";
    public const string SortIndex = "__i";
    public const string SortOrientation = "__o";
}

internal static class QueryFieldConditionGroupPropertiesNames
{
    public const string Entity = "__cdgr";
    public const string Operator = "__o";
}

internal static class QueryFieldConditionPropertiesNames
{
    public const string Entity = "__cd";
    public const string Operator = "__o";
    public const string Value = "__v";
    public const string FieldName = "__n";
    public const string DataType = "__t";
    public const string IsCollection = "__l";
}