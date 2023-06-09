FROM node:18.7.0 AS node-builder
EXPOSE 80
WORKDIR /node
COPY ./GameEvaluator.Web /node
RUN npm install
RUN npm run build

FROM nginx:latest AS runtime
COPY --from=node-builder /node/build /usr/share/nginx/html
