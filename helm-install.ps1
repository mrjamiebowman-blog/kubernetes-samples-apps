# clear terminal
Clear-Host

# create namespace
kubectl create namespace mrjb

# install helm charts
helm install api-customers .\charts\app-identityserver\ -n mrjb
helm install api-customers .\charts\consumer-customers\ -n mrjb
helm install api-customers .\charts\api-customers\ -n mrjb
helm install api-customers .\charts\api-orders\ -n mrjb
