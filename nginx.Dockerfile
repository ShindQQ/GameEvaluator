FROM nginx:latest	
RUN apt update
RUN apt-get install nano -y

COPY ./nginx.local.conf /etc/nginx/nginx.conf