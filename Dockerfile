# ===========================
# 1) Build Stage
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY *.sln ./

# Copy csproj files for all projects (improves layer caching)
COPY VisionX.ATMS.Web/*.csproj VisionX.ATMS.Web/
COPY VisionX.BusinessAccessLayer.Repositories/*.csproj VisionX.BusinessAccessLayer.Repositories/
COPY VisionX.BusinessEntities.Repositories/*.csproj VisionX.BusinessEntities.Repositories/
COPY VisionX.DataAccessLayer.Repositories/*.csproj VisionX.DataAccessLayer.Repositories/
COPY VisionX.Interface.Repositories/*.csproj VisionX.Interface.Repositories/

# Restore packages
RUN dotnet restore

# Copy everything else
COPY . .

# Publish ONLY the web project
RUN dotnet publish "VisionX.ATMS.Web/VisionX.ATMS.Web.csproj" -c Release -o /app/publish


# ===========================
# 2) Runtime Stage
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Render provides PORT environment variable at runtime.
# This ensures Kestrel binds on 0.0.0.0 and correct port.
ENV ASPNETCORE_URLS=http://+:${PORT:-10000}
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published output from build stage
COPY --from=build /app/publish .

# Optional: expose default port (Render detects it automatically)
EXPOSE 10000

# ENTRYPOINT should match your web project DLL name
ENTRYPOINT ["dotnet", "VisionX.ATMS.Web.dll"]