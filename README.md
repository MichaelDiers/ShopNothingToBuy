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

| Operation      | Status Code | Result                           | Description                                  |
| -------------- | ----------- | -------------------------------- | -------------------------------------------- |
| all            | 401         | UnauthorizedResult               | api key missing or invalid                   |
| all            | 400/404     | BadRequestResult/NotFoundResult  | validation of route parameter or body failed | 
| DELETE (all)   | 204         | NoContentResult                  | all items are deleted                        |
| DELETE (all)   | 500         | StatusCodeResult(500)            | deletion failed with interal server error    |
| DELETE (item)  | 204         | NoContentResult                  | item is deleted                              |
| DELETE (item)  | 404         | NotFoundResult                   | item does not exist                          |
| DELETE (item)  | 500         | StatusCodeResult(500)            | internal server error                        |
| GET (all)      | 200         | OkObjectResult(items)            | read all operation succeeded                 |
| GET (all)      | 500         | StatusCodeResult(500)            | read all fails                               |
| GET (item)     | 200         | OkObjectResult(item)             | item found                                   |
| GET (item)     | 404         | NotFoundResult                   | item does not exist                          |
| GET (item)     | 500         | StatusCodeResult(500)            | internal server error                        |
| POST (item)    | 201         | CreatedResult(item)              | item is created                              |
| POST (item)    | 409         | ConflictResult                   | item already exists                          |
| POST (item)    | 500         | StatusCodeResult(500)            | internal server error                        |
| PUT (item)     | 200         | OkObjectResult(item) or OkResult | item is updated                              |
| PUT (item)     | 404         | NotFoundResult                   | item does not exist                          |
| PUT (item)     | 409         | ConflictObjectResult(item)       | update failed                                |
| PUT (item)     | 500         | StatusCodeResult(500)            | internal server error                        |

