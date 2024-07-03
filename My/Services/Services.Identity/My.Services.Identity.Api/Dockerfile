FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /My.Microservices

# Copy csproj and restore as distinct layers
COPY ./Common/My.Application/*.csproj ./Common/My.Application/
COPY ./Common/My.CrossCuttingConcerns/*.csproj ./Common/My.CrossCuttingConcerns/
COPY ./Common/My.Domain/*.csproj ./Common/My.Domain/
COPY ./Common/My.Infrastructure/*.csproj ./Common/My.Infrastructure/
RUN dotnet restore ./Common/My.Application/My.Application.csproj
RUN dotnet restore ./Common/My.Infrastructure/My.Infrastructure.csproj

COPY ./Services.Identity/My.Services.Identity/*.csproj ./Services.Identity/My.Services.Identity/
RUN dotnet restore ./Services.Identity/My.Services.Identity/My.Services.Identity.csproj

COPY ./Services.Identity/My.Services.Identity.Api/*.csproj ./Services.Identity/My.Services.Identity.Api/
RUN dotnet restore ./Services.Identity/My.Services.Identity.Api/My.Services.Identity.Api.csproj

# Copy everything else and build My
COPY . ./
RUN dotnet publish ./Services.Identity/My.Services.Identity.Api/My.Services.Identity.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /My.Microservices
COPY --from=build-env /My.Microservices/out .

ENTRYPOINT ["dotnet", "My.Services.Identity.Api.dll"]