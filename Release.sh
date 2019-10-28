#!/bin/bash
NEXT_VERSION=$1
PROJECT_FOLDER=$2
BINARY_NAME=$3
cd "${PROJECT_FOLDER}"
dotnet build && dotnet pack -p:PackageVersion=${NEXT_VERSION}
cd bin/Debug && zip -r release.zip .