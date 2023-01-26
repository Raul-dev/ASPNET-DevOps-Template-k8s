
## Start project localy
for Windows
```
./start.ps1

or

cd src
docker-compose -f docker-compose.misc.yml up
docker-compose --env-file ./.env.win up
docker-compose -f docker-compose.elk.yml up
```

For Linux
```
cd src
docker-compose -f docker-compose.misc.yml up
docker-compose --env-file ./.env.linux -f docker-compose.yml -f docker-compose.override.yml up
docker-compose -f docker-compose.elk.yml up
```
## to launch a project
https://localhost

https://localhost/ref


## GitLab CI/CD compose files in folder ./deployment/gitlabci/
Build machine: https://docker.neva.loc as example

## reset chrome cahe
```
chrome://net-internals/#hsts
chrome:restart
```

Check Self signet certificate for domain
```
cd deployment/TestSSL
docker-compose --env-file ./.env_linux -f docker-compose.sslnginx.yml up
```

## Build frontend only
```
docker-compose build --force-rm --no-cache --progress plain frontend
```
