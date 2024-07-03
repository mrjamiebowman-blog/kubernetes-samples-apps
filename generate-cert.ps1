Clear-Host

# dotnet generate self-signing certificate
# https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-web-app-dotnet-core-sign-in#create-and-upload-a-self-signed-certificate

# dotnet generate self-signing certificate
dotnet dev-certs https -ep ./certificate.crt --trust

