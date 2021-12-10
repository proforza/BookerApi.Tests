FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish

COPY . /build
WORKDIR /build
RUN dotnet publish "BookerApi.Tests/BookerApi.Tests.csproj" -c Release -o /publish -r linux-x64

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine
WORKDIR /tests
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "test", "BookerApi.Tests.dll"]