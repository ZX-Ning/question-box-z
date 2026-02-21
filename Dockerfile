FROM ghcr.io/zx-ning/web-dev-env AS builder

WORKDIR /app
COPY . .
ENV CI=true
RUN rm -rf build \
    && mkdir build  \
    && cd QuestionBox.Server \
    && dotnet restore \
    && dotnet publish -f net10.0 -o ../build \
    && mkdir -p db \
    && cd .. \
    && cd QuestionBox.Client \
    && pnpm install \
    && pnpm vite build --outDir ../build/wwwroot --emptyOutDir

FROM mcr.microsoft.com/dotnet/aspnet:10.0.3-alpine3.22

WORKDIR /
COPY --from=builder /app/build/ /app
WORKDIR /app
CMD ["dotnet", "./QuestionBox.Server.dll"]