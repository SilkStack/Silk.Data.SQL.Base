﻿using System.Collections.Generic;

namespace Silk.Data.SQL
{
	public enum SqlBaseType
	{
		/// <summary>
		/// Integer value that can be 0, 1 or (optionally) NULL.
		/// </summary>
		Bit,
		/// <summary>
		/// 1 byte integer.
		/// </summary>
		TinyInt,
		/// <summary>
		/// 2 byte integer.
		/// </summary>
		SmallInt,
		/// <summary>
		/// 4 byte integer.
		/// </summary>
		Int,
		/// <summary>
		/// 8 byte integer.
		/// </summary>
		BigInt,
		/// <summary>
		/// Numeric type with fixed precision and scale.
		/// </summary>
		Decimal,
		/// <summary>
		/// Approximate number type.
		/// </summary>
		Float,
		Date,
		Time,
		DateTime,
		Text,
		Binary,
	}

	public class SqlDataType
	{
		public SqlBaseType BaseType { get; }
		public int[] Parameters { get; }

		public SqlDataType(SqlBaseType baseType, params int[] parameters)
		{
			BaseType = baseType;
			Parameters = parameters;
		}

		private static Dictionary<SqlBaseType, SqlDataType> _staticDataTypes =
			new Dictionary<SqlBaseType, SqlDataType>()
			{
				{ SqlBaseType.Bit, new SqlDataType(SqlBaseType.Bit) },
				{ SqlBaseType.TinyInt, new SqlDataType(SqlBaseType.TinyInt) },
				{ SqlBaseType.SmallInt, new SqlDataType(SqlBaseType.SmallInt) },
				{ SqlBaseType.Int, new SqlDataType(SqlBaseType.Int) },
				{ SqlBaseType.BigInt, new SqlDataType(SqlBaseType.BigInt) },
				{ SqlBaseType.Date, new SqlDataType(SqlBaseType.Date) },
				{ SqlBaseType.Time, new SqlDataType(SqlBaseType.Time) },
				{ SqlBaseType.DateTime, new SqlDataType(SqlBaseType.DateTime) },
				{ SqlBaseType.Text, new SqlDataType(SqlBaseType.Text) }
			};

		public const int FLOAT_MAX_PRECISION = 24;
		public const int DOUBLE_MAX_PRECISION = 53;

		public static SqlDataType Bit() => _staticDataTypes[SqlBaseType.Bit];
		public static SqlDataType TinyInt() => _staticDataTypes[SqlBaseType.TinyInt];
		public static SqlDataType SmallInt() => _staticDataTypes[SqlBaseType.SmallInt];
		public static SqlDataType Int() => _staticDataTypes[SqlBaseType.Int];
		public static SqlDataType BigInt() => _staticDataTypes[SqlBaseType.BigInt];
		public static SqlDataType Date() => _staticDataTypes[SqlBaseType.Date];
		public static SqlDataType Time() => _staticDataTypes[SqlBaseType.Time];
		public static SqlDataType DateTime() => _staticDataTypes[SqlBaseType.DateTime];
		public static SqlDataType Text() => _staticDataTypes[SqlBaseType.Text];
		public static SqlDataType Text(int maxLength) => new SqlDataType(SqlBaseType.Text, maxLength);
		public static SqlDataType Binary(int maxLength) => new SqlDataType(SqlBaseType.Binary, maxLength);
		public static SqlDataType Decimal(int precision = 18, int scale = 0) => new SqlDataType(SqlBaseType.Decimal, precision, scale);
		public static SqlDataType Float(int precision = FLOAT_MAX_PRECISION) => new SqlDataType(SqlBaseType.Float, precision);
	}
}
