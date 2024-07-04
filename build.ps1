Clear-Host

# environment variables
$NUGET_USER = [System.Environment]::GetEnvironmentVariable('NUGET_USER','user')
$NUGET_PAT = [System.Environment]::GetEnvironmentVariable('NUGET_PAT','user')

# docker build
$VERSION = 'latest'

# # customers api
# docker build --no-cache -f "CustomersApi\Dockerfile" `
#     --build-arg NUGET_USER=$NUGET_USER --build-arg NUGET_PAT=$NUGET_PAT `
#     --label "company=mrjamiebowman" `
#     -t mrjamiebowman/app-k8s-customers-api:$VERSION .

# # customers consumer
# docker build --no-cache -f "CustomersConsumer\Dockerfile" `
#     --build-arg NUGET_USER=$NUGET_USER --build-arg NUGET_PAT=$NUGET_PAT `
#     --label "company=mrjamiebowman" `
#     -t mrjamiebowman/app-k8s-customers-consumer:$VERSION .

# orders api
docker build --no-cache -f "OrdersApi\Dockerfile" `
    --build-arg NUGET_USER=$NUGET_USER --build-arg NUGET_PAT=$NUGET_PAT `
    --label "company=mrjamiebowman" `
    -t mrjamiebowman/app-k8s-orders-api:$VERSION .

# # identity server
# docker build --no-cache -f "IdentityServer\Dockerfile" `
#     --build-arg NUGET_USER=$NUGET_USER --build-arg NUGET_PAT=$NUGET_PAT `
#     --label "company=mrjamiebowman" `
#     -t mrjamiebowman/app-k8s-identityserver:$VERSION .

# display built images
docker images | findstr mrjamiebowman 