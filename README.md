# About
These tests were created to demonstrate my skills in testing REST APIs.

API used: **https://restful-booker.herokuapp.com/apidoc/index.html**

Frameworks used: .NET5, nUnit.

## Installation

Just clone this repo locally, open a cmd/powershell window from the directory that contains BookerApi.Tests.csproj and finally run dotnet command:

```bash
dotnet test BookerApi.Tests.csproj -c Release -r Results --logger trx
```

Or if you want to run compiled .dll just download the build artifacts and run command (in the root folder):

```bash
dotnet test .\BookerApi.Tests.dll
```

Result .trx file will appear in the /Results folder. The most CI/CD tools (TeamCity, Jenkins, Azure DevOps etc.) support this type of file.
I wanted to create more readable and best-looking html reports using Allure, but unfortunately it supports only .NET 4.7.\* and .net core 2.\*.

## Tests description

Tests order:

  1. **HealthCheckTests** - to be shure that API is available
  2. **CreateTokenTests** - to create an auth token (will be used only once, all other tests will use basic auth)
  3. **TokenAuthTests** - test PATCH request using token-based auth
  4. **BookingValidationsTests** - get a random booking, validate fields and creating booking with incorrect data 
  5. **BookingTests** - full cycle of a Booking entity. Create -> update -> patch -> delete.

## License
[MIT](https://choosealicense.com/licenses/mit/)