FROM quay.io/keycloak/keycloak:15.0.2

WORKDIR /app
COPY ./blazor-keycloak/identity-provider/blazor-realm.json .

ENV KEYCLOAK_USER=admin
ENV KEYCLOAK_PASSWORD=<Strong!Passw0rd>
ENV PROXY_ADDRESS_FORWARDING=true
ENV KEYCLOAK_IMPORT=/app/blazor-realm.json
