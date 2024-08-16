#!/usr/bin/sh
cd QuestionBox.Client
npx vite build
cd ../QuestionBox.Server
dotnet run -c release --environment Production