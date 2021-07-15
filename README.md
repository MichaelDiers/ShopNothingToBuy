# ShopNothingToBuy

## ProductsApi.AzureFunctions [Link](ProductsApi.AzureFunctions)

API provides CRUD operations for products used in the NothingToBuy Shop.

### Techs
- .NET Core 3.1
- Microsoft Azure Functions
- Microsoft Azure CosmosDb
- xUnit
- Postman

## StockApi.WebApi.NetCore [Link](StockApi.WebApi.NetCore)

API provides CRUD operations for stock items used in the NothingToBuy Shop.

### Techs
- .NET Core 3.1
- Web-API mit ASP.NET Core
- Redis Cloud Database
- xUnit
- Postman

## API default behaviour

| Operation      | Status Code | Result                     | Description                               |
| -------------- | ----------- | -------------------------- | ----------------------------------------- |
| DELETE (all)   | 204         | NoContentResult            | all items are deleted                     |
| DELETE (all)   | 500         | StatusCodeResult           | deletion failed with interal server error |
| DELETE (item)  | 204         | NoContentResult            | item is deleted                           |
| DELETE (item)  | 404         | NotFoundResult             | item does not exist                       |
| GET (item)     | 200         | OkObjectResult(item)       | item found                                |
| GET (item)     | 404         | NotFoundResult             | item does not exist                       |
| POST           | 201         | CreatedResult(item)        | item is created                           |
| POST           | 409         | ConflictResult             | item already exists                       |
| PUT            | 200         | OkObjectResult(item)       | item is updated                           |
| PUT            | 404         | NotFoundResult             | item does not exist                       |
| PUT            | 409         | ConflictObjectResult(item) | update failed                             |

