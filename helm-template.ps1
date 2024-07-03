# clear terminal
Clear-Host

# helm template (-f charts/values.prod.yaml)
if ($args[0] -eq 'dev') {
    helm template apicustomers .\charts\api-customers\ -n sn -f charts/api-customers/values.dev.yaml --debug
} elseif ($args[0] -eq 'prod') {
    helm template apicustomers .\charts\api-customers\ -n sn -f charts/api-customers/values.prod.yaml --debug
} else {
    helm template apicustomers .\charts\api-customers\ -n sn --debug
}
