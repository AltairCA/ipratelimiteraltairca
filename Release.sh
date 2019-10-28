#!/bin/bash
export NEXT_VERSION=$1
export PROJECT_FOLDER=$2
export BINARY_NAME=$3
cd "${PROJECT_FOLDER}"
dotnet build && dotnet pack -p:PackageVersion=${NEXT_VERSION}
cd bin/Debug && zip -r release.zip .