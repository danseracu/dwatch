FROM golang:alpine AS builder
WORKDIR /app

# COPY go.* ./

# RUN go mod download

# COPY *.go ./

# RUN go build -o /dwatch_server

ENV GO111MODULE=on \
    CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=amd64

COPY ./server.go .

RUN go build -o server ./server.go

FROM gcr.io/distroless/base-debian10

WORKDIR /

COPY --from=builder /app/server /dwatch_server

EXPOSE 8080
ENTRYPOINT ["/dwatch_server"]