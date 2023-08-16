namespace Ecubytes.QueryModels;

public class QueryFieldSort
{
    private QueryFieldSort() { }

    public string FieldName { get; internal set; } = null!;
    public SortOrientation SortOrientation { get; internal set; }
    public int SortIndex { get; internal set; }

    public static QueryFieldSort Build(string fieldName)
    {
        return Build(fieldName, SortOrientation.Ascendent, 0);
    }

    public static QueryFieldSort Build(string fieldName, int sortIndex)
    {
        return Build(fieldName, SortOrientation.Ascendent, sortIndex);
    }

    public static QueryFieldSort Build(string fieldName, SortOrientation sortOrientation)
    {
        return Build(fieldName, sortOrientation, 0);
    }

    public static QueryFieldSort Build(string fieldName, SortOrientation sortOrientation, int sortIndex)
    {
        ArgumentNullException.ThrowIfNull(fieldName, nameof(fieldName));

        QueryFieldSort field = new()
        {
            FieldName = fieldName,
            SortOrientation = sortOrientation,
            SortIndex = sortIndex
        };

        return field;
    }
}