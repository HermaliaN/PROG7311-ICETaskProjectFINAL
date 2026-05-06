FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PROG7311_ICEFinal.csproj", "./"]
RUN dotnet restore "PROG7311_ICEFinal.csproj"

COPY . .
RUN dotnet publish "PROG7311_ICEFinal.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "PROG7311_ICEFinal.dll"]