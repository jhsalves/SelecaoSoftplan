FROM mcr.microsoft.com/dotnet/core/sdk:2.2.401
 
WORKDIR /app
 
COPY . .
 
RUN dotnet restore
 
RUN dotnet publish -o /publish/
 
WORKDIR /publish
 
ENTRYPOINT ["dotnet", "API2.dll"]