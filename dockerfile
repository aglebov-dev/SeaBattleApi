FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS builder
ARG CONFIGURATION=Release
ARG APPLICATION=SeaBattle.Api

COPY ./src .

RUN dotnet restore -v quiet;
RUN dotnet build -c ${CONFIGURATION};
RUN dotnet publish "/SeaBattle.Api/SeaBattle.Api.csproj" --no-build -c ${CONFIGURATION} -o /publish;

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
EXPOSE 20270

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "SeaBattle.Api.dll"]