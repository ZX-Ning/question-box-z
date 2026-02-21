FROM ghcr.io/zx-ning/web-dev-env AS builder

WORKDIR /app
COPY . .
ENV CI=true
RUN bash publish.sh

FROM mcr.microsoft.com/dotnet/aspnet:10.0.3-noble

WORKDIR /
COPY --from=builder /app/build/ /app
WORKDIR /app
CMD ["dotnet", "./QuestionBox.Server.dll"]