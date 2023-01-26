# build environment
FROM node:alpine as build
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
ARG API_URL
COPY Frontend/WebSPA/page/package.json ./
COPY Frontend/WebSPA/page/package-lock.json ./
RUN npm ci --silent
RUN npm install react-scripts@3.4.1 -g --silent
COPY ./Frontend/WebSPA/page ./
RUN REACT_APP_API_URL=$API_URL npm run build

# production environment
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html

EXPOSE 80

COPY srv/nginx/backend.conf.template /etc/nginx/templates/default.conf.template

RUN mkdir -p /usr/share/nginx/html/ref/
COPY Frontend/WebSPA/nginx/links.html /usr/share/nginx/html/ref/index.html
