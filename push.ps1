Clear-Host

# api: customers
docker push mrjamiebowman/app-k8s-customers-api:latest

# consumers: customers
docker push mrjamiebowman/app-k8s-customers-consumers:latest

# api: orders
docker push mrjamiebowman/app-k8s-orders-api:latest

# identityserver
docker push mrjamiebowman/app-k8s-identityserver:latest