#!/bin/bash

dotnet restore --packages /kubeless/packages /kubeless/

dotnet Kubeless.WebAPI.dll