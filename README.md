#   Dapper.TableValuedParameter [![Build status](https://ci.appveyor.com/api/projects/status/3t2nxjcyy61krfql/branch/master?svg=true)](https://ci.appveyor.com/project/ayberkcanturk/dapper-tablevaluedparameter)


## Installation [![NuGet version](https://badge.fury.io/nu/Dapper.TableValuedParameter.svg)](https://badge.fury.io/nu/Dapper.TableValuedParameter)

It's as easy as `PM> Install-Package Dapper.TableValuedParameter` from [nuget](http://nuget.org/packages/Dapper.TableValuedParameter)

## Quick Start

Pass your "Tvp" class instance as a parameter to Dapper Query method.

Just an example;

```csharp

	    using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString))
	    {
		IEnumerable<TvpDto> tvpDto = new List<TvpDto>()
		{
		... //This is the parameter which is going to be projected to your table-valued parameter.
		};
		    
		return db.Query<Author>("dbo.GetAuthor", new Tvp("@ParameterName", "dbo.UserDefinedTypeName", tvpDto), commandType: CommandType.StoredProcedure).ToList(); 
            }
```
