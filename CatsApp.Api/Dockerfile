#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CatsApp.Api/CatsApp.Api.csproj", "CatsApp.Api/"]
COPY ["CatsApp.Application/CatsApp.Application.csproj", "CatsApp.Application/"]
COPY ["CatsApp.Domain/CatsApp.Domain.csproj", "CatsApp.Domain/"]
COPY ["CatsApp.Infrastructure/CatsApp.Infrastructure.csproj", "CatsApp.Infrastructure/"]
RUN dotnet restore "CatsApp.Api/CatsApp.Api.csproj"
COPY . .
WORKDIR "/src/CatsApp.Api"
RUN dotnet build "CatsApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CatsApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatsApp.Api.dll"]