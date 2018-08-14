# Overview

Low-level library for creating SQL queries and statements. This API is not really intended to be used in main application logic but used as an implementation detail of SQL APIs.

The API surface exposed by this library is intended to be as generic and general purpose as possible. A cross-section of features available in most modern SQL databases exposed in this library while server specific libraries (called providers) can provide more server specific features and functionality.

## Goals

The main purpose of this library is to enable libraries to operate on SQL data without being bound to a specific database provider.

My personal goal is to support as much functionality that is exposed across the following database servers as possible and support as many varied SQL use cases as possible on these platforms:

* Microsoft SQL Server (SQL Server, Express and Azure)
* Postgresql
* SQLite 3
* MySQL/MariaDB

## Usage

### Provider

To execute SQL queries a project needs a provider that implements `IDataProvider`. There is no default `IDataProvider` implementation in this library.

### Query Expressions

Authoring SQL queries is accomplished using query expressions - an API inspired by `System.Linq.Expressions`.

Just like `System.Linq.Expressions` most of the expression APIs you'll need to use are static methods on a single type: `QueryExpression`.

#### Select

Build SELECT statements using `QueryExpression.Select`.

    var selectExpression = QueryExpression.Select(
        new [] {
            QueryExpression.Column("Id"),
            QueryExpression.Column("DisplayName")
        },
        from: QueryExpression.Table("Users"),
        where: QueryExpression.Compare(QueryExpression.Column("Id"), ComparisonOperator.AreEqual, QueryExpression.Value(5)),
        limit: QueryExpression.Value(1)
    );

#### Insert

Build INSERT statements using `QueryExpression.Insert`. The `Insert` API allows for bulk inserts by providing multiple collections of field values.

    var insertExpression = QueryExpression.Insert(
        QueryExpression.Table("Users"),
        new [] { "Id", "DisplayName", "EmailAddress" },
        new object[] { 1, "JohnDoe", "john@doe.com" },
        new object[] { 2, "JaneDoe", "jane@doe.com" }
    );

#### Update

Build UPDATE statements using `QueryExpression.Update`.

    var updateExpression = QueryExpression.Update(
        QueryExpression.Table("Users"),
        QueryExpression.Compare(QueryExpression.Column("Id"), ComparisonOperator.AreEqual, QueryExpression.Value(5)),
        QueryExpression.Assign("DisplayName", "JoeBloggs"),
        QueryExpression.Assign("EmailAddress", "joe@bloggs.com")
    );

#### Delete

Build DELETE statements using `QueryExpression.Delete`.

    var deleteExpression = QueryExpression.Delete(
        QueryExpression.Table("Users"),
        QueryExpression.Compare(QueryExpression.Column("Id"), ComparisonOperator.AreEqual, QueryExpression.Value(5))
    );

#### Transactions

Execute multiple statements in a transaction using `QueryExpression.Transaction`.

    var transactionExpression = QueryExpression.Transaction(insertExpression, updateExpression);

# Data Storage Types

The following common SQL storage types are exposed:

* Bit	- A `0`, `1` or `NULL` value.
* TinyInt - 1 byte integer.
* SmallInt - 2 byte integer.
* Int - 4 byte integer.
* BigInt - 8 byte integer.
* Float - Approximate number type, accepts precision and scale options.
* Decimal - Fixed precision numeric type.
* Date
* Time
* DateTime
* Text - Unicode text, accepts a max length option to make (N)VARCHAR(*length*).
* Binary - Variable length binary storage, requires a max length option.

# Planned Features

* More SQL functions support
* More math expression support
* Define and install SQL functions/procedures using the `QueryExpression` API.

# Limitations

`Silk.Data.SQL.Base` is designed to be a general purpose library that provides compatibility across different SQL database dialects.

As such only a cross-section of functionality is exposed in this library. Dialect/engine specific functionality should be implemented in/alongside data provider libraries.

## Parameterized Queries

All value expressions are written to SQL queries as parameters. There's no API for providing values in-place in an SQL statement.

## Raw SQL

`Silk.Data.SQL.Base` provides no API for executing raw SQL - raw SQL is considered to be dialect specific. While it's entirely possible to write very general SQL statements that will work across most SQL dialects it's a very error prone approach.

## SQL functions

Only a cross section of SQL functions across the main target database engines. Currently supported functions are:

* DISTINCT (not really a function but implemented as one)
* COUNT
* RANDOM

## Transactions

Nested transactions and named transactions are not supported.

## Stored Procedures

The stored procedure API only allows arguments passed in sequence without names. Named sproc parameters are not available.

Currently there is no API for OUT sproc parameters.

# License

`Silk.Data.SQL.Base` is made available under the MIT license.