# About
These tests were created to demonstrate my skills in testing REST APIs.

API used: **https://restful-booker.herokuapp.com/apidoc/index.html**

Frameworks used: .NET5, nUnit.

## Installation

Just clone this repo locally, open a cmd/powershell window from the directory that contains BookerApi.Tests.csproj and finally run dotnet command:

```powershell
dotnet test BookerApi.Tests.csproj -c Release -r Results --logger trx
```
Result .trx file will appear in the /Results folder. The most CI/CD tools (TeamCity, Jenkins, Azure DevOps etc.) support this type of file.
## Tests description

TODO

## Contributing
TODO

## License
[MIT](https://choosealicense.com/licenses/mit/)