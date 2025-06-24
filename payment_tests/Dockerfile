FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./payment_backend/payment_backend.csproj ./payment_backend/
COPY ./payment_tests/payment_tests.csproj ./payment_tests/

RUN dotnet restore ./payment_tests/payment_tests.csproj

COPY ./payment_backend/ ./payment_backend
COPY ./payment_tests/ ./payment_tests

RUN dotnet build ./payment_tests/payment_tests.csproj -c Release

FROM build AS tester
WORKDIR /app
CMD ["dotnet", "test", "./payment_tests/payment_tests.csproj"]
