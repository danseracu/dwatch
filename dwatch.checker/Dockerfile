#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
#USER app
#WORKDIR /app
FROM busybox:1.35.0-uclibc as busybox
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release

RUN apk add clang binutils musl-dev build-base zlib-static openssl

WORKDIR /src
COPY dwatch.checker.csproj .
RUN dotnet restore "./dwatch.checker.csproj" -r linux-musl-$TARGETARCH
COPY . .
#RUN dotnet build "./dwatch.checker.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN dotnet publish "./dwatch.checker.csproj" --self-contained -r linux-musl-$TARGETARCH -c $BUILD_CONFIGURATION -o /app/publish 
RUN rm /app/publish/*.dbg

#FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-cbl-mariner2.0-distroless-aot
#WORKDIR /app/publish
#
#COPY --from=build /app/publish .
#USER app
#
#ENTRYPOINT ["./dwatch.checker"]

#FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
#FROM scratch AS runtime
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled

WORKDIR /app

COPY --from=build /app/publish .
# COPY --from=ep76/openssl-static:latest /usr/bin/openssl /usr/bin/
# COPY --from=ep76/openssl-static:latest /usr/lib /usr/include /usr/

# COPY --from=busybox /bin/sh /bin/sh

ENTRYPOINT ["./dwatch.checker"]