Clear-Host

# environment variables
$NUGET_USER = [System.Environment]::GetEnvironmentVariable('NUGET_USER','user')
$NUGET_PAT = [System.Environment]::GetEnvironmentVariable('NUGET_PAT','user')

# docker build
$VERSION = 'latest'

docker build --no-cache -f "CustomersApi\Dockerfile" `
    --build-arg NUGET_USER=$NUGET_USER --build-arg NUGET_PAT=$NUGET_PAT `
    --label "company=mrjamiebowman" `
    -t mrjamiebowman/app-k8s-customer-api:$VERSION .

# display built images
docker images | findstr mrjamiebowman 