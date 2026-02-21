#!/bin/bash
set -e
rm -rf build
mkdir build
cd QuestionBox.Server
dotnet restore
dotnet publish -f net10.0 -o ../build
cd ..

cd QuestionBox.Client
pnpm install
pnpm vite build --outDir ../build/wwwroot --emptyOutDir
