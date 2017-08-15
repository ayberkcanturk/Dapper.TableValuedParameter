#   Dapper.TableValuedParameter [![Build status](https://ci.appveyor.com/api/projects/status/3t2nxjcyy61krfql/branch/master?svg=true)](https://ci.appveyor.com/project/ayberkcanturk/dapper-tablevaluedparameter)


## Installation [![NuGet version](https://badge.fury.io/nu/Dapper.TableValuedParameter.svg)](https://badge.fury.io/nu/Dapper.TableValuedParameter)

It's as easy as `PM> Install-Package Dapper.TableValuedParameter` from [nuget](http://nuget.org/packages/Dapper.TableValuedParameter)

## Quick Start

Pass your "Tvp" class instance as a parameter to Dapper Query method.

Just an example;

```sql
	CREATE TYPE dbo.SaleOrderDetailType AS TABLE  
	( UnitPrice MONEY, OrderQty INT ) 
    
    	CREATE PROCEDURE [dbo].[GetSaleOrderDetail] @SaleOrderDetail dbo.[SaleOrderDetailType] READONLY
	AS
	BEGIN
			SELECT 
					SOD.UnitPrice, 
					SOD.OrderQty
			FROM 
					SaleOrderDetail AS SOD WITH(NOLOCK) 
					WHERE SOD.UnitPrice = @UnitPrice AND SOD.OrderQty = @OrderQty
	END    
```

```csharp
	public class SaleOrderDetailTypeDto()
	{
		[Column(Name:"SellingPrice")]
		[Map(SqlDbType.Money)] //.NET framework data type has more than one possibility in SqlDbType such as decimal, money,smallmoney etc. So we have to explictly it.
		public decimal Price { get; set; } 

		public int OrderQty { get; set; } //It  will be automatically mapped to SqlDbType.Int
	}

	using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString))
	{
		IEnumerable<SaleOrderDetailTypeDto> tvpDto = new List<SaleOrderDetailTypeDto>()
		{
		... //This is the parameter which is going to be projected to your table-valued parameter.
		};

		return db.Query<SaleOrderDetail>("dbo.GetSaleOrderDetail", new Tvp("@SaleOrderDetail", "dbo.SaleOrderDetailType", tvpDto), commandType: CommandType.StoredProcedure).ToList(); 
    }
```
