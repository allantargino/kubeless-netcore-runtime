#!/bin/bash

if [ -e /kubeless/*.csproj ]
then
    dotnet restore --packages /kubeless/packages /kubeless/
fi

dotnet Kubeless.WebAPI.dll