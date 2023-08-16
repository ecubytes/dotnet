namespace Ecubytes.QueryModels;

public class QueryFieldConditionGroup
{
    internal QueryRequest QueryRequest { get; set; } = null!;
    private List<QueryFieldCondition>? conditions;
    private List<QueryFieldConditionGroup>? conditionGroups;

    public LogicalOperators Operator { get; internal set; }
    public bool IsEmpty
    {
        get
        {
            return !this.Conditions.Any() && !this.ConditionGroups.Any();
        }
    }

    public QueryFieldConditionGroup SetOperator(LogicalOperators logicalOperator)
    {
        this.Operator = logicalOperator;
        return this;
    }

    public List<QueryFieldCondition> Conditions
    {
        get
        {
            if (conditions == null)
                conditions = new List<QueryFieldCondition>();

            return conditions;
        }
    }
    public List<QueryFieldConditionGroup> ConditionGroups
    {
        get
        {
            if (conditionGroups == null)
                conditionGroups = new List<QueryFieldConditionGroup>();

            return conditionGroups;
        }
    }

    public QueryFieldCondition AddCondition(string fieldName, object value, RelationalOperators relationalOperator)
    {
        var condition = QueryFieldCondition.BuildCondition(fieldName, value, relationalOperator);
        Conditions.Add(condition);
        return condition;
    }

    public QueryRequest EndGroup()
    {
        return QueryRequest;
    }

    public static QueryFieldConditionGroup BuildConditionGroup(LogicalOperators logicalOperator)
    {
        QueryFieldConditionGroup group = new()
        {
            Operator = logicalOperator
        };

        return group;
    }

}