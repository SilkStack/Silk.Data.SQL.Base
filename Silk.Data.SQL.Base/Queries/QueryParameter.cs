namespace Silk.Data.SQL.Queries
{
	public class QueryParameter
	{
		public string Name { get; }
		public object Value { get; set; }

		public QueryParameter(string name)
		{
			Name = name;
		}
	}
}
