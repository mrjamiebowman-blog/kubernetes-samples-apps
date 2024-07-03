Clear-Host

# docker build
$VERSION = 'dev'

# dos2unix
dos2unix docker/mssql/db.sh
dos2unix docker/mssql/entrypoint.sh

# # microservice: hangfire
# docker build --no-cache -f "docker\mssql\Dockerfile" `
#     --label "company=MrJB" `
#     -t mrjamiebowman/app-hangfire-mssql:$VERSION ./docker/mssql/

# # display built images
# docker images | findstr mrjb 

# docker compose build
docker-compose build --no-cache