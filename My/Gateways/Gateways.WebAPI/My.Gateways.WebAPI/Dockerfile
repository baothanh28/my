FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /My.Microservices

# Copy csproj and restore as distinct layers
COPY ./Common/My.Application/*.csproj ./Common/My.Application/
COPY ./Common/My.CrossCuttingConcerns/*.csproj ./Common/My.CrossCuttingConcerns/
COPY ./Common/My.Domain/*.csproj ./Common/My.Domain/
COPY ./Common/My.Infrastructure/*.csproj ./Common/My.Infrastructure/
RUN dotnet restore ./Common/My.Application/My.Application.csproj
RUN dotnet restore ./Common/My.Infrastructure/My.Infrastructure.csproj

COPY ./Gateways.WebAPI/My.Gateways.WebAPI/*.csproj ./Gateways.WebAPI/My.Gateways.WebAPI/
RUN dotnet restore ./Gateways.WebAPI/My.Gateways.WebAPI/My.Gateways.WebAPI.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./Gateways.WebAPI/My.Gateways.WebAPI/My.Gateways.WebAPI.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /My.Microservices
COPY --from=build-env /My.Microservices/out .

ENTRYPOINT ["dotnet", "My.Gateways.WebAPI.dll"]