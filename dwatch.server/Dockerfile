FROM --platform=${BUILDPLATFORM:-linux/amd64} golang:alpine AS builder
WORKDIR /app

ARG TARGETARCH
ARG BUILDPLATFORM

ENV GO111MODULE=on \
    CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=${TARGETARCH}

COPY ./server.go .

RUN go build -o server ./server.go

#FROM gcr.io/distroless/base-debian12
FROM --platform=${BUILDPLATFORM:-linux/amd64} scratch

WORKDIR /

COPY --from=builder /app/server /dwatch_server

EXPOSE 8080
ENTRYPOINT ["/dwatch_server"]