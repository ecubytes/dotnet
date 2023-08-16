using System.Collections;

namespace Ecubytes.QueryModels;
public class QueryFieldCondition
    {
        private bool isDataTypeSet = false;
        private bool isCollectionSet = false;
        private bool isCollection = false;
        private string? dataType = null;

        public string? FieldName { get; internal set; }
        public object? Value { get; internal set; }
        public RelationalOperators Operator { get; set; }
        public QueryFieldCondition SetFieldName(string fieldName)
        {
            ArgumentNullException.ThrowIfNull(fieldName, nameof(fieldName));
            this.FieldName = fieldName;
            return this;
        }
        public QueryFieldCondition SetValue(object value)
        {
            this.Value = value;
            return this;
        }
        public QueryFieldCondition SetDataType(string dataType)
        {
            this.DataType = dataType;
            return this;
        }
        public QueryFieldCondition SetIsCollection(bool isCollection)
        {
            this.IsCollection = isCollection;
            return this;
        }
        public QueryFieldCondition SetOperator(RelationalOperators relationalOperator)
        {
            this.Operator = relationalOperator;
            return this;
        }
        public bool IsCollection
        {
            get
            {
                if (!isCollectionSet)
                {
                    if (Value == null)
                        isCollection = false;
                    else
                        isCollection = Value is ICollection;
                }
                return isCollection;
            }
            internal set
            {
                isCollectionSet = true;
                isCollection = value;
            }
        }
        public string DataType
        {
            get
            {
                if (!isDataTypeSet || dataType == null)
                {
                    if (Value == null)
                        dataType = "String";
                    else
                        dataType = Value.GetType().Name;
                }
                return dataType;
            }
            internal set
            {
                if (dataType != value)
                {
                    isDataTypeSet = true;
                    dataType = value;
                }
            }
        }

        public static QueryFieldCondition BuildCondition(string fieldName, object value, RelationalOperators relationalOperator)
        {
            QueryFieldCondition field = new QueryFieldCondition();
            if (value != null)
            {                
                field.FieldName = fieldName;
                field.Value = value;

                if (!field.IsCollection)
                    field.DataType = value.GetType().Name;
                else
                {
                    Type? elementType = null;

                    elementType = value?.GetType()?.GetElementType();
                    if(elementType == null)
                        elementType = value?.GetType().GetGenericArguments()[0];                    
                    if(elementType == null)
                        elementType = typeof(String);

                    field.DataType = elementType.Name;
                }
                field.Operator = relationalOperator;                
            }

            return field;
        }
    }