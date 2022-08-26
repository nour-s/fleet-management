# Fleet Management

This repository contains the code for the Fleet Management system as requested by Trendyol.

## Stack

The project uses .NET 6 Web API and Entity Framework for Database.

## Architecture

The project follows the `Domain Driven` architecture with the following concepts:

- CQRS (Command Query Responsibility Segregation)
- Test Driven Development (As much as possible)
- Clean Architecture

I used an in-memory database for the sake of simplicity. The database can be easily changed to any other database by changing the connection string in the appsettings.json file.

The database is automatically seeded with the data provided in the requirements.md file.
Notice that the database will be reset on every run allowing you to test the system with different data every time.

Check the docs folder for the class diagram and state diagram.

## How to run

#### Option 1: Using Visual Studio or Visual Studio Code:

You can open the solution using either of them. You need the .NET 6 SDK to build and run the project which is automatically available with VS 2019 or higher.

#### Option 2: Using Docker:

There is a docker-compose file in the docker folder of the project. You can run the following command to start the project:

```bash
cd docker
docker-compose up -p fleet-management
```

#### Option 3: Using dotnet command line:

You can run the following command to start the project:

```bash
dotnet run --project ./src/WebApi/WebApi.csproj
```

Notice that the `dotnet cli` is available when installing the .NET SDK.

#### Running tests:

You can either use the built-in test runner of your IDE or run the following command:

```bash
dotnet test
```

```
// Output

Passed! - Failed: 0, Passed: 14, Skipped: 0, Total: 14, Duration: 59 ms - Domain.Tests.dll (net6.0)

Passed! - Failed: 0, Passed:  4, Skipped: 0, Total: 4, Duration: 15 ms - Application.Tests.dll (net6.0)

Passed! - Failed: 0, Passed:  1, Skipped: 0, Total: 1, Duration: < 1 ms - WebApi.Tests.dll (net6.0)
```

#### Doing a real request:

As provided in the requirements, the system is a REST API. You can use any tool you want to make requests to the API.
If you followed the running instructions above, the API will be available at http://localhost:5000.

You can find a file called `.http/post-delivery.http` which requires a VS Code extension called [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client). You can use this file to make a request to the API.

You can also use the following command:

**Bash:**

```bash
curl --request POST \
  --url http://localhost:5000/shipments/deliver \
  --header 'content-type: application/json' \
  --header 'user-agent: vscode-restclient' \
  --data '{"vehicle": "34 TL 34","route": [{"deliveryPoint": 1,"deliveries": [{"barcode": "P7988000121"},{"barcode": "P7988000122"},{"barcode": "P7988000123"},{"barcode": "P8988000121"},{"barcode": "C725799"}]},{"deliveryPoint": 2,"deliveries": [{"barcode": "P8988000123"},{"barcode": "P8988000124"},{"barcode": "P8988000125"},{"barcode": "C725799"}]},{"deliveryPoint": 3,"deliveries": [{"barcode": "P9988000126"},{"barcode": "P9988000127"},{"barcode": "P9988000128"},{"barcode": "P9988000129"},{"barcode": "P9988000130"}]}]}'
```

**PowerShell:**

```powershell
Invoke-RestMethod -Method Post -Uri http://localhost:5000/shipments/deliver -Headers @{'content-type'='application/json';'user-agent'='vscode-restclient'} -Body '{"vehicle": "34 TL 34","route": [{"deliveryPoint": 1,"deliveries": [{"barcode": "P7988000121"},{"barcode": "P7988000122"},{"barcode": "P7988000123"},{"barcode": "P8988000121"},{"barcode": "C725799"}]},{"deliveryPoint": 2,"deliveries": [{"barcode": "P8988000123"},{"barcode": "P8988000124"},{"barcode": "P8988000125"},{"barcode": "C725799"}]},{"deliveryPoint": 3,"deliveries": [{"barcode": "P9988000126"},{"barcode": "P9988000127"},{"barcode": "P9988000128"},{"barcode": "P9988000129"},{"barcode": "P9988000130"}]}]}'
```

##### Checking the state of a shipment

Since I'm using an in-memory database, you won't be able to query the database using any tool, so I added a simple endpoint to query the state of a shipment using the barcode.

You can find a file called `.http/get-shipment-state.http` to run such a request, or use the following command:

```
curl --request GET --url http://localhost:5000/shipments?barcode/P7988000121 --header 'content-type: application/json'

// response
{
  "barcode": "P7988000121",
  "state": "Unloaded"
}
```

#### Logging the skipped shpiments

I chose to log the skipped shipments to the stdout rather than the database for simplicity.
The app will print the following when you run the POST request.

```
+-------------+----------------------------------------------------------------------------+
| Barcode     | Message                                                                    |
+-------------+----------------------------------------------------------------------------+
| P8988000121 | Package P8988000121 can't be unloaded to Branch                            |
+-------------+----------------------------------------------------------------------------+
| C725799     | Sack C725799 can't be unloaded to Branch because it is not the destination |
+-------------+----------------------------------------------------------------------------+
| P9988000126 | A Package P9988000126 without a sack can't be unloaded to TransferCentre   |
+-------------+----------------------------------------------------------------------------+
| P9988000127 | A Package P9988000127 without a sack can't be unloaded to TransferCentre   |
+-------------+----------------------------------------------------------------------------+

```

### Notes:

#### Discrepancy

You will notice that the response is as expected except for Sack `C725799`. The requirements expect this Sack to remain unloaded at the first route with delivery point 1, but get unloaded at the third route with delivery point 3.

The reason for this discrepancy is that I designed the system to deliver all shipments as requested and then query the database after the whole batch is processed and persisted (Especially using CQRS).

Not knowing that the system is expected to do a kind of dry run and return what would be the state of the package if it was sent to the provided location, and since the mentioned sack is mentioned twice in the request, it was unloaded at the later route but yet still reported as unloaded at the first route too.

I chose not to fix that as it will require fundamental changes to the architecture and I don't think it will be of any value to the assessment. I'm happy to discuss what would be the alternative if required.

#### Assumptions/Decisions I made

- Packages' barcode start with "P"
- Sacks' barcode start with "C"
- The package takes the state `Loaded Into Sack` when loaded into the sack even though the requirements mentioned:

  `Shipments take “created” state when they are first created, switch to “loaded” state when they are loaded into a sack, and switch to “unloaded” when they are unloaded at the delivery point.`

- The Sack takes the state “loaded” when it is loaded into a vehicle explicitly, there was no mention of such a case (apart from listing it in the Sack state table).
- I use DomainException to represent any invalid operation that is not allowed by the business, but in our case the requirement is to skip them (such as unloading a shipment to the wrong destination)
- The system is missing some edge case handling such as unloading an unloaded shipment
- I haven't added any unit tests for the `GET /shipments?barcode=<barcode>` endpoint as it is not part of the requirement.

#### Tools I used

I used mermaid to create the flow charts using just code. If you notice the docs folder, you will see some charts.
I also used it to create the class diagram for reference.

`Mermaid` is supported by Github if you inserted it in Markdown files. You can also install a VS Code client that can render Mermaid diagrams in Markdown files.
Check this article for more information [Mermaid in Github](https://github.blog/2022-02-14-include-diagrams-markdown-files-mermaid/)
