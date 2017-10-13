﻿using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;

namespace Silk.Data.SQL.Queries
{
	public class QueryResult : IDisposable
	{
		private readonly DbCommand _command;
		private readonly DbDataReader _reader;

		public bool HasRows => _reader.HasRows;

		public int FieldCount => _reader.FieldCount;

		public QueryResult(DbCommand command, DbDataReader reader)
		{
			_command = command;
			_reader = reader;
		}

		public void Dispose()
		{
			_reader.Dispose();
			_command.Dispose();
		}

		public bool GetBoolean(int ordinal)
			=> _reader.GetBoolean(ordinal);

		public byte GetByte(int ordinal)
			=> _reader.GetByte(ordinal);

		//public byte[] GetBytes(int ordinal)
		//{
		//	return _reader.GetBytes(ordinal)
		//}

		public char GetChar(int ordinal)
			=> _reader.GetChar(ordinal);

		public DateTime GetDateTime(int ordinal)
			=> _reader.GetDateTime(ordinal);

		public decimal GetDecimal(int ordinal)
			=> _reader.GetDecimal(ordinal);

		public double GetDouble(int ordinal)
			=> _reader.GetDouble(ordinal);

		public Type GetFieldType(int ordinal)
			=> _reader.GetFieldType(ordinal);

		public float GetFloat(int ordinal)
			=> _reader.GetFloat(ordinal);

		public Guid GetGuid(int ordinal)
			=> _reader.GetGuid(ordinal);

		public short GetInt16(int ordinal)
			=> _reader.GetInt16(ordinal);

		public int GetInt32(int ordinal)
			=> _reader.GetInt32(ordinal);

		public long GetInt64(int ordinal)
			=> _reader.GetInt64(ordinal);

		public string GetName(int ordinal)
			=> _reader.GetName(ordinal);

		public Stream GetStream(int ordinal)
			=> _reader.GetStream(ordinal);

		public string GetString(int ordinal)
			=> _reader.GetString(ordinal);

		public bool IsDBNull(int ordinal)
			=> _reader.IsDBNull(ordinal);

		public bool NextResult()
			=> _reader.NextResult();

		public Task<bool> NextResultAsync()
			=> _reader.NextResultAsync();

		public bool Read()
			=> _reader.Read();

		public Task<bool> ReadAsync()
			=> _reader.ReadAsync();
	}
}
