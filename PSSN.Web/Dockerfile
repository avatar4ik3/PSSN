FROM node:latest as builder
WORKDIR /PSSN.Web
COPY PSSN.Web/package.json /PSSN.Web/package.json
RUN npm install --only=prod
COPY PSSN.Web/ /PSSN.Web
RUN npm run build

FROM nginx:alpine
COPY --from=builder /PSSN.Web/build /usr/share/nginx/html
RUN rm /etc/nginx/conf.d/default.conf
COPY PSSN.Web/nginx.conf /etc/nginx/conf.d
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]