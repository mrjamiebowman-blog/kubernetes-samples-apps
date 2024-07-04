# terminal
Clear-Host

# helm template (-f charts/values.prod.yaml)
if ($args[0] -eq 'dev') {
    helm template apicustomers .\CustomersApi\charts\api-customers\ -n mrjamiebowman -f CustomersApi/charts/api-customers/values.dev.yaml --debug
} elseif ($args[0] -eq 'prod') {
    helm template apicustomers .\CustomersApi\charts\api-customers\ -n mrjamiebowman -f CustomersApi/charts/api-customers/values.prod.yaml --debug
} else {
    helm template apicustomers .\CustomersApi\charts\api-customers\ -n mrjamiebowman --debug
}


# helm template (-f charts/values.prod.yaml)
if ($args[0] -eq 'dev') {
    helm template consumerscustomers .\CustomersConsumer\charts\consumer-customers\ -n mrjamiebowman -f CustomersConsumer/charts/consumer-customers/values.dev.yaml --debug
} elseif ($args[0] -eq 'prod') {
    helm template consumerscustomers .\CustomersConsumer\charts\consumer-customers\ -n mrjamiebowman -f CustomersConsumer/charts/consumer-customers/values.prod.yaml --debug
} else {
    helm template consumerscustomers .\CustomersConsumer\charts\consumer-customers\ -n mrjamiebowman --debug
}


# helm template (-f charts/values.prod.yaml)
if ($args[0] -eq 'dev') {
    helm template apiorders .\OrdersApi\charts\api-orders\ -n mrjamiebowman -f OrdersApi/charts/api-orders/values.dev.yaml --debug
} elseif ($args[0] -eq 'prod') {
    helm template apiorders .\OrdersApi\charts\api-orders\ -n mrjamiebowman -f OrdersApi/charts/api-orders/values.prod.yaml --debug
} else {
    helm template apiorders .\OrdersApi\charts\api-orders\ -n mrjamiebowman --debug
}


# helm template (-f charts/values.prod.yaml)
if ($args[0] -eq 'dev') {
    helm template identityserver .\IdentityServer\charts\app-identityserver\ -n mrjamiebowman -f IdentityServer/charts/app-identityserver/values.dev.yaml --debug
} elseif ($args[0] -eq 'prod') {
    helm template identityserver .\IdentityServer\charts\app-identityserver\ -n mrjamiebowman -f IdentityServer/charts/app-identityserver/values.prod.yaml --debug
} else {
    helm template identityserver .\IdentityServer\charts\app-identityserver\ -n mrjamiebowman --debug
}

