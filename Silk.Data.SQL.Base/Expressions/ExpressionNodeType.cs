namespace Silk.Data.SQL.Expressions
{
	public enum ExpressionNodeType
	{
		Query,
		Binary,
		SchemaComponent,
		Value,
		DbFunction,
		Alias,
		Join,
		Assignment,
		ColumnDefinition,
		Modifier,
		Extension
	}
}
