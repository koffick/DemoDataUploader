FROM postgres:latest

USER root

COPY docker/docker-entrypoints/init-user-db.sh /docker-entrypoint-initdb.d/init-user-db.sh