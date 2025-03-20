FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FinanceApp.sln ./
COPY Domain/Domain.csproj Domain/
COPY Application/Application.csproj Application/
COPY FinanceApp/FinanceApp.csproj FinanceApp/

RUN dotnet restore FinanceApp.sln

COPY . .

WORKDIR /src/FinanceApp
RUN dotnet publish FinanceApp.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "FinanceApp.dll", "--interactive"]