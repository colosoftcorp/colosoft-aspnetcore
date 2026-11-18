# Colosoft AspNetCore

Project with the definition of colosoft's AspNetCore.


| Package | NuGet | Downloads |
| ------- | ------------ | --------- |
| [Colosoft.AspNetCore.Diagnostics](https://www.nuget.org/packages/Colosoft.AspNetCore.Diagnostics/) | [![Colosoft.AspNetCore.Diagnostics](https://img.shields.io/nuget/v/Colosoft.AspNetCore.Diagnostics.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Diagnostics/) | [![Colosoft.AspNetCore.Diagnostics](https://img.shields.io/nuget/dt/Colosoft.AspNetCore.Diagnostics.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Diagnostics/) |
| [Colosoft.AspNetCore.Middleware](https://www.nuget.org/packages/Colosoft.AspNetCore.Middleware/) | [![Colosoft.AspNetCore.Middleware](https://img.shields.io/nuget/v/Colosoft.AspNetCore.Middleware.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Middleware/) | [![Colosoft.AspNetCore.Middleware](https://img.shields.io/nuget/dt/Colosoft.AspNetCore.Middleware.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Middleware/) |
| [Colosoft.AspNetCore.Mvc](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc/) | [![Colosoft.AspNetCore.Mvc](https://img.shields.io/nuget/v/Colosoft.AspNetCore.Mvc.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc/) | [![Colosoft.AspNetCore.Mvc](https://img.shields.io/nuget/dt/Colosoft.AspNetCore.Mvc.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc/) |
| [Colosoft.AspNetCore.Mvc.Abstractions](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc.Abstractions/) | [![Colosoft.AspNetCore.Mvc.Abstractions](https://img.shields.io/nuget/v/Colosoft.AspNetCore.Mvc.Abstractions.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc.Abstractions/) | [![Colosoft.AspNetCore.Mvc.Abstractions](https://img.shields.io/nuget/dt/Colosoft.AspNetCore.Mvc.Abstractions.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc.Abstractions/) |
| [Colosoft.AspNetCore.Mvc.NewtonsoftJson](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc.NewtonsoftJson/) | [![Colosoft.AspNetCore.Mvc.NewtonsoftJson](https://img.shields.io/nuget/v/Colosoft.AspNetCore.Mvc.NewtonsoftJson.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc.NewtonsoftJson/) | [![Colosoft.AspNetCore.Mvc.NewtonsoftJson](https://img.shields.io/nuget/dt/Colosoft.AspNetCore.Mvc.NewtonsoftJson.svg)](https://www.nuget.org/packages/Colosoft.AspNetCore.Mvc.NewtonsoftJson/) |



## Security fixes

### 3.0.0-alpha.2 — sorting text checked by the model binder (Colosoft.AspNetCore.Mvc)

Up to `3.0.0-alpha.1`, the `ISorting` model binder took any text of the `sort` query string parameter as field names,
so a field like `(SELECT SLEEP(3))` reached the query layer, which wrote it as SQL in the `ORDER BY` clause.

- Each field must be a plain identifier (letters, digits and underscore, not starting with a digit, dotted for a
  prefix) and each direction `asc` or `desc` (`field`, `field:desc` or `field desc`). Anything else is a model state
  error on `sort` and the binding fails, so an `[ApiController]` answers `400`. `ISortingBuilder.AddField` and
  `AddExpression` throw `SortingFormatException` for the same cases.
- `SortingTranslationAttribute` can be set on the action method or on its `ISorting` parameter, and the binder reads it
  from there; before, it was read from the parameter type, where it could not be set, so the translation never ran.
- `BaseSortingTranslation` keeps only `asc` or `desc` as the direction and throws `SortingFormatException` for any other
  text, instead of appending the client text to the translated field.

The binder is a first line of defense: the query must still accept only the fields of its own map (for example
`OrderBy(query, input, allowedColumns)` of `Colosoft.Data.Dapper` 3.0.0-beta.3 or `Colosoft.Application.Dapper`
3.0.0-alpha.2).
