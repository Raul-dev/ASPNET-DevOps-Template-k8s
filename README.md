
## Start project localy
for Windows
```
./start.ps1

or

cd src
docker-compose -f docker-compose.misc.yml up -d
docker-compose --env-file ./.env.win up
docker-compose -f docker-compose.elk.yml up -d
```

For Linux
```
cd src
docker-compose -f docker-compose.misc.yml up
docker-compose --env-file ./.env.linux -f docker-compose.yml -f docker-compose.override.yml up
docker-compose -f docker-compose.elk.yml up
```
## ## Project URL
https://localhost

https://localhost/ref


## GitLab CI/CD compose files in folder ./deployment/gitlabci/
Build machine: https://docker.neva.loc as example

## reset chrome cache
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
