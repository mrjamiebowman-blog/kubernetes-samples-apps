Clear-Host

# api: customers
docker push mrjamiebowman/app-k8s-customer-api:latest

# consumers: customers
docker push mrjamiebowman/app-k8s-customer-consumers:latest

# # api: orders
# docker push mrjamiebowman/app-k8s-orders-api:latest

# # identityserver
# docker push mrjamiebowman/app-k8s-identityserver:latest