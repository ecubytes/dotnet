using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecubytes.QueryResult.AspNetCore.Mvc.ModelBinder;

public class QueryRequestModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        return Task.Factory.StartNew(() =>
        {
            BindModel(bindingContext);
        });
    }
    
    public virtual void BindModel(ModelBindingContext bindingContext)
    {
        // Model binding is not set, thus AspNet5 will keep looking for other model binders.
        if (!bindingContext.ModelType.Equals(typeof(QueryRequest)))
        {
            //return ModelBindingResult.NoResult;
            return;
        }

        var values = bindingContext.ValueProvider;

        var page = values.GetValue(QueryRequestPropertiesNames.Page);

        int? _page = null;
        if (page.Any())
        {
            int __page = 0;
            if (Parse<int>(page, out __page))
                _page = __page;
        }

        var pageSize = values.GetValue(QueryRequestPropertiesNames.PageSize);
        int? _pageSize = null;
        if (pageSize.Any())
        {
            int __pageSize = 0;
            if (Parse<int>(pageSize, out __pageSize))
                _pageSize = __pageSize;
        }

        var searchValue = values.GetValue(QueryRequestPropertiesNames.SearchValue);
        string? _searchValue = null;
        Parse<string>(searchValue, out _searchValue);

        var queryRequest = QueryRequest.Builder().SetPage(_page).SetPageSize(_pageSize).SetSearchValue(_searchValue);

        ParseSortFields(queryRequest, values);
        //Start Root ConditionGroup
        ParseConditionGroups(queryRequest.MainConditionGroup, $"{QueryFieldConditionGroupPropertiesNames.Entity}", values);


        bindingContext.Result = ModelBindingResult.Success(queryRequest);
    }

    private static bool ParseConditionGroups(QueryFieldConditionGroup conditionGroup, string prefix, IValueProvider values)
    {
        var operatorGroup = values.GetValue($"{prefix}.{QueryFieldConditionGroupPropertiesNames.Operator}");
        int _operatorGroup = 0;
        if (!Parse<int>(operatorGroup, out _operatorGroup)) return false;

        conditionGroup.SetOperator((LogicalOperators)_operatorGroup);

        int counter = 0;
        while (true)
        {
            // Parses Name value.
            var fieldName = values.GetValue($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{counter}].{QueryFieldConditionPropertiesNames.FieldName}");
            string? _fieldName = null;
            if (!Parse<string>(fieldName, out _fieldName)) break;

            var operatorCondition = values.GetValue($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{counter}].{QueryFieldConditionPropertiesNames.Operator}");
            int _operatorCondition = 0;
            Parse<int>(operatorCondition, out _operatorCondition);

            var dataType = values.GetValue($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{counter}].{QueryFieldConditionPropertiesNames.DataType}");
            string? _dataType = null;
            Parse<string>(dataType, out _dataType);


            var isCollection = values.GetValue($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{counter}].{QueryFieldConditionPropertiesNames.IsCollection}");
            bool _isCollection = false;
            Parse<bool>(isCollection, out _isCollection);

            if (_dataType != null)
            {
                var valueCondition = values.GetValue($"{prefix}.{QueryFieldConditionPropertiesNames.Entity}[{counter}].{QueryFieldConditionPropertiesNames.Value}");
                string? _valueCondition = null;
                object? valueConditionObjet = null;
                if (Parse<string>(valueCondition, out _valueCondition))
                    valueConditionObjet = ParseValue(_dataType, _valueCondition, _isCollection);

                var condition = conditionGroup.AddCondition(_fieldName, valueConditionObjet, (RelationalOperators)_operatorCondition);
                condition.SetDataType(_dataType);
                condition.SetIsCollection(_isCollection);
            }

            // Increments counter to keep processing columns.
            counter++;
        }

        int counterInnerGroup = 0;
        while (ParseConditionGroups(conditionGroup, $"{prefix}.{QueryFieldConditionGroupPropertiesNames.Entity}[{counterInnerGroup}]", values))
        {
            counterInnerGroup++;
        }

        return true;
    }

    private static object? ParseValue(string dataType, object? value, bool isCollection)
    {
        object? valueFilter = null;

        if (value != null)
        {
            if (isCollection)
            {
                string[] splitValues = value.ToString()?.Split('~') ?? Array.Empty<string>();
                if (dataType == "Guid")
                    valueFilter = splitValues.Select(p => Guid.Parse(p)).ToList();
                else
                    valueFilter = splitValues.Select(p => Convert.ChangeType(value, Type.GetType($"System.{dataType}"))).ToList();
            }
            else
            {
                valueFilter = Convert.ChangeType(value, Type.GetType($"System.{dataType}"));
            }
        }

        return valueFilter;
    }

    private static void ParseSortFields(QueryRequest queryRequest, IValueProvider values)
    {
        int counter = 0;
        while (true)
        {
            // Parses Name value.
            var fieldName = values.GetValue($"{QueryFieldSortPropertiesNames.Entity}[{counter}].{QueryFieldSortPropertiesNames.FieldName}");
            string? _fieldName = null;
            Parse<string>(fieldName, out _fieldName);
            if (!Parse<string>(fieldName, out _fieldName)) break;

            var sortOrder = values.GetValue($"{QueryFieldSortPropertiesNames.Entity}[{counter}].{QueryFieldSortPropertiesNames.SortIndex}");
            int _sortOrder = 0;
            //if (!Parse<string>(columnField, out _columnField)) break;
            Parse<int>(sortOrder, out _sortOrder);

            // Parses Orderable value.
            var sortOrientation = values.GetValue($"{QueryFieldSortPropertiesNames.Entity}[{counter}].{QueryFieldSortPropertiesNames.SortOrientation}");
            int _sortOrientation = 0;
            Parse<int>(sortOrientation, out _sortOrientation);

            queryRequest.AddFieldSort(_fieldName, (SortOrientation)_sortOrientation, _sortOrder);

            // Increments counter to keep processing columns.
            counter++;
        }
    }


    /// <summary>
    /// Parses a possible raw value and transforms into a strongly-typed result.
    /// </summary>
    /// <typeparam name="ElementType">The expected type for result.</typeparam>
    /// <param name="value">The possible request value.</param>
    /// <param name="result">Returns the parsing result or default value for type is parsing failed.</param>
    /// <returns>True if parsing succeeded, False otherwise.</returns>
    private static bool Parse<ElementType>(ValueProviderResult value, out ElementType result)
    {
        result = default(ElementType);

        //if (value == null) return false;
        if (string.IsNullOrWhiteSpace(value.FirstValue)) return false;

        try
        {
            result = (ElementType)Convert.ChangeType(value.FirstValue, typeof(ElementType));
            return true;
        }
        catch { return false; }
    }
}