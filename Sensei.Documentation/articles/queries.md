# Resolve HTTP queries

Sensei provide helpers to handle common actions over HTTP requests,
in specific it provide:

- [Filtering](#filtering)
- [Sorting](#sorting)
- [Including](#including)
- [Pagination](#pagination)

Before run our queries we need to obtain a [Query](xref:Sensei.AspNet.QueryProcessor.Query)
object that is where our filters will be applied.

To obtain that object we use the [QueryProcessor](xref:Sensei.AspNet.QueryProcessor.QueryProcessor)
that is available into the DI system.

As example:
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
apply filters with a fluent syntax. As example:
 
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

Filters work with a lucene-like syntax with some differences to adapt it to an SQL data source.

As example:
```http request
// description:%coffee% AND (price:>10 OR quantity:<=100)

GET /categories?filters=description%3A%25coffee%25%20AND%20(price%3A%3E10%20OR%20quantity%3A%3C%3D100)
```

It will be translate in something like:

```sql
SELECT * FROM Categories WHERE Description = '%coffee%' AND (Price > 10 OR Quantity <= 100)
```

> [!WARNING]
> At the moment filtering only work with server-side queries.
> This because for string we use the DbFunctions.Like method.

### Syntax

A series of examples for understand how filter queries work:

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
