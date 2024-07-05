# stage 1
FROM node:latest as angular 
WORKDIR /app
#COPY ["/package*.json","./"]
COPY . .
RUN npm install 
RUN npm run build 

FROM nginx:alpine
VOLUME /var/cache/nginx
COPY --from=angular /app/dist/challenge-b3-angular/  /usr/share/nginx/html
COPY ./config/nginx.conf /etc/nginx/conf.d/default.conf 

# stage 2
# FROM nginx:alpine
# COPY --from=node /app/dist/challenge-b3-angular /usr/share/nginx/html
# FROM node:latest as angular
# WORKDIR /app/src/
# COPY ["/package*.json","./"]
# RUN npm install
# COPY . .
# RUN npm install --legacy-peer-deps true 
#RUN sh -c "npm install -g @angular/cli"
#RUN sh -c "ng serve --host 0.0.0.0 --port 4200"
# RUN npm run build
# EXPOSE 4200
#RUN exit 1