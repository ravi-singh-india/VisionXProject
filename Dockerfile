FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only project file for web project to leverage cache
COPY VisionX.ATMS.Web/*.csproj VisionX.ATMS.Web/
# If other projects are referenced by csproj, you can copy their csproj too (optional)
COPY VisionX.BusinessAccessLayer.Repositories/*.csproj VisionX.BusinessAccessLayer.Repositories/
COPY VisionX.BusinessEntities.Repositories/*.csproj VisionX.BusinessEntities.Repositories/
COPY VisionX.DataAccessLayer.Repositories/*.csproj VisionX.DataAccessLayer.Repositories/
COPY VisionX.Interface.Repositories/*.csproj VisionX.Interface.Repositories/

# Restore NuGet packages only for the web project (explicit path)
RUN dotnet restore "VisionX.ATMS.Web/VisionX.ATMS.Web.csproj"

# Copy everything
COPY . .

# Publish the web project
RUN dotnet publish "VisionX.ATMS.Web/VisionX.ATMS.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:${PORT:-10000}
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["dotnet", "VisionX.ATMS.Web.dll"]