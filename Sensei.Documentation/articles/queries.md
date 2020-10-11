# Resolve HTTP queries

Sensei provides a mechanism to perform the most common actions related to
HTTP requests. The system provides a series of predefined helpers and,
if necessary, it can be extended with customized actions.

The default helpers are:

- [Filtering](#filtering)
- [Sorting](#sorting)
- [Including](#including)
- [Pagination](#pagination)

The actions are performed on a [Query](xref:Sensei.AspNet.QueryProcessor.Query)
type object which contains the IQueryable interface on which to execute the
queries and IServiceContainer which allows access to the DI system.

To get a [Query](xref:Sensei.AspNet.QueryProcessor.Query) instance we can use
the [QueryProcessor](xref:Sensei.AspNet.QueryProcessor.QueryProcessor) that is
available inside the DI system and start the
 [Query](xref:Sensei.AspNet.QueryProcessor.Query) through the `Start()` method.

Example:

```c#
public class CategoryController : ControllerBase
{
    private MyDbContext _dbContext;
    private QueryProcessor _queryProcessor;

    public CategoryController(MyDbContext dbContext, QueryProcessor queryProcessor)
    {
        _dbContext = dbContext;
        _queryProcessor = queryProcessor;
    }

    public ActionResult Get()
    {
        var query = QueryProcessor.Start(_dbContext.Categories);
        // ...
        return query.Queryable;
    }
}
```

Once the [Query](xref:Sensei.AspNet.QueryProcessor.Query) is instantiated you can
apply filters with a fluent syntax, as example:
 
```c#
public ActionResult<Paginator<Category>> Get(
    [FromQuery] Filtering filtering,
    [FromQuery] Sorting sorting,
    [FromQuery] Pagination pagination,
    [FromQuery] Including including
)
{
    return QueryProcessor
        .Start(DbSet)
            .ApplyFilters(filtering)
            .ApplySorting(sorting)
            .ApplyIncluding(including)
            .ApplyPagination(pagination);
}
```

## Filtering

Filters work through lucene-like syntax with some limitations due to the
SQL data source.

Filters are applied to strings using the `LIKE` operator which means that it is
case insensitive, with the exception of some SQL servers such as PostgreSQL
where `LIKE` is case sensitive.

The wildcard characters used to filter strings depend on the SQL server used
(as above they are processed through the `LIKE` operator).

Example of a request with a filter applied:
```test
description:%coffee% AND (price:>10 OR quantity:<=100)
```
```http request
GET /categories?filters=description%3A%25coffee%25%20AND%20(price%3A%3E10%20OR%20quantity%3A%3C%3D100)
```

The previous query is converted to an SQL query similar to:

```sql
SELECT * FROM Categories WHERE Description = '%coffee%' AND (Price > 10 OR Quantity <= 100)
```

> [!WARNING]
> Filters currently only work on queries that are resolved server-side,
> due to the use of the DbFunctions.Like method to process strings.

### Syntax

Here is a series of examples to understand how filter queries work:

- Where the field `name` match the value `coffee`
  ```text
  name:coffee
  ```
- Where the field `name` doesn't match the value `coffee`
  ```text
  NOT name:coffee
  ```
- here the field `author` match the value `John Smith`
  ```text
  author:"John Smith"
  ```
- Where the field `description` contain the value `sugar`
  ```text
  description:%sugar%
  ```
- Where the field `description` starts with the value `sugar`
  ```text
  description:%sugar
  ```
- Where the field `description` end with the value `sugar`
  ```text
  description:sugar%
  ```
- Where the field `author` is not `null`
  ```text
  _exists_:author
  ```  
- Where the field `author` is `null`
  ```text
  NOT _exists_:author
  ```
- Where the field `price` is greater than `10`
  ```text
  price:>10
  ```
- Where the field `price` is greater or equal than `10`
  ```text
  price:>=10
  ```
- Where the field `price` is less than `20`
  ```text
  price:<20
  ```
- Where the field `price` is less or equal than `20`
  ```text
  price:<=20
  ```
- Where the field `price` is greater then `10` and less than `20`
  ```text
  price:[10 TO 20]
  ```
- Where the field `price` is greater then `10` and less than `20` and
  `description` contain `sugar` or `name` is not equal to `coffee`
   ```text
   price:[10 TO 20] AND (description:%sugar% OR NOT name:coffee)
   ```

## Sorting

## Including

## Pagination
