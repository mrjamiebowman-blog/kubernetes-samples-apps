Clear-Host

# environment variables
$NUGET_USER = [System.Environment]::GetEnvironmentVariable('NUGET_USER','user')
$NUGET_PAT = [System.Environment]::GetEnvironmentVariable('NUGET_PAT','user')

# customers api
docker run -it --rm `
    -e NUGET_USER="$NUGET_USER" -e NUGET_PAT="$NUGET_PAT" `
    mrjamiebowman/app-k8s-customer-api:latest /bin/bash
