FROM rabbitmq:latest

USER root

## change timezone
ENV TZ=Europe/Moscow
RUN rm /etc/localtime && \
    ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
    echo $TZ > /etc/timezone

COPY docker/docker-entrypoints/rabbitmq.conf /etc/rabbitmq/rabbitmq.conf
COPY docker/docker-entrypoints/definitions.json /etc/rabbitmq/definitions.json

RUN rabbitmq-plugins enable rabbitmq_management