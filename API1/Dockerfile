FROM microsoft/dotnet:sdk AS build-env
 
WORKDIR /app
 
COPY . .
 
RUN dotnet restore
 
RUN dotnet publish -o /publish/
 
WORKDIR /publish
 
ENTRYPOINT ["dotnet", "API1.dll"]