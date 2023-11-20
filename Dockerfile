ARG PARENT_VERSION=1.5.0-dotnet6.0

# Development
FROM defradigital/dotnetcore-development:$PARENT_VERSION AS development

ARG PARENT_VERSION
ARG PACKAGE_FEED_URL
ARG PACKAGE_FEED_USERNAME
ARG PACKAGE_FEED_PAT

LABEL uk.gov.defra.parent-image=defra-dotnetcore-development:${PARENT_VERSION}

RUN mkdir -p /home/dotnet/EST.MIT.Approvals/EST.MIT.Approvals.Data/ /home/dotnet/EST.MIT.Approvals.SeedProvider/

COPY --chown=dotnet:dotnet ./docker-nuget.config ./nuget.config

COPY --chown=dotnet:dotnet ./EST.MIT.Approvals/EST.MIT.Approvals.Data/*.csproj ./EST.MIT.Approvals/EST.MIT.Approvals.Data/
RUN dotnet restore ./EST.MIT.Approvals/EST.MIT.Approvals.Data/EST.MIT.Approvals.Data.csproj

COPY --chown=dotnet:dotnet ./EST.MIT.Approvals.SeedProvider/*.csproj ./EST.MIT.Approvals.SeedProvider/
RUN dotnet restore ./EST.MIT.Approvals.SeedProvider/EST.MIT.Approvals.SeedProvider.csproj

COPY --chown=dotnet:dotnet ./EST.MIT.Approvals.SeedProvider/ ./EST.MIT.Approvals.SeedProvider/
COPY --chown=dotnet:dotnet ./EST.MIT.Approvals/EST.MIT.Approvals.Data/ ./EST.MIT.Approvals/EST.MIT.Approvals.Data/

RUN dotnet publish ./EST.MIT.Approvals.SeedProvider/ -c Release -o /home/dotnet/out

# Production
FROM defradigital/dotnetcore:$PARENT_VERSION AS production

ARG PARENT_VERSION
ARG PARENT_REGISTRY

LABEL uk.gov.defra.parent-image=defra-dotnetcore-development:${PARENT_VERSION}

COPY --from=development /home/dotnet/out/ ./

CMD dotnet ./EST.MIT.Approvals.SeedProvider.dll