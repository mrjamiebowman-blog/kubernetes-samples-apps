Clear-Host

# identityserver
docker run -it --rm -p 5000:5000 `
    mrjamiebowman/app-k8s-identityserver:latest /bin/bash

# # customers api
# docker run -it --rm -p 5010:8080 `
#     mrjamiebowman/app-k8s-customer-api:latest /bin/bash


# # orders api
# docker run -it --rm -p 5010:8080 `
#     mrjamiebowman/app-k8s-orders-api:latest /bin/bash

# # customers consumer
# docker run -it --rm -p 5010:8080 `
#     mrjamiebowman/app-k8s-customer-consumer:latest /bin/bash

