FROM fedora:43

WORKDIR /app

RUN dnf in git fish curl dotnet-sdk-10.0 nodejs24 pnpm python3 -y \
    && dnf clean all -y

CMD ["fish"]