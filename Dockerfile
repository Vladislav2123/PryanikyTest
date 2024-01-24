FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/PryanikyTest.API/PryanikyTest.API.csproj", "Presentation/PryanikyTest.API/"]
COPY ["src/Core/PryanikyTest.Application/PryanikyTest.Application.csproj", "Core/PryanikyTest.Application/"]
COPY ["src/Core/PryanikyTest.Domain/PryanikyTest.Domain.csproj", "Core/PryanikyTest.Domain/"]
COPY ["src/Infrastructure/PryanikyTest.DAL/PryanikyTest.DAL.csproj", "Infrastructure/PryanikyTest.DAL/"]
RUN dotnet restore "Presentation/PryanikyTest.API/PryanikyTest.API.csproj"
COPY ./src .
WORKDIR "/src/Presentation/PryanikyTest.API"
RUN dotnet publish "PryanikyTest.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
CMD ["dotnet", "PryanikyTest.API.dll"]

EXPOSE 5000
EXPOSE 5001