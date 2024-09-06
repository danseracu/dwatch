FROM --platform=${BUILDPLATFORM:-linux/amd64} golang:alpine AS builder
WORKDIR /app

ARG TARGETARCH
ARG BUILDPLATFORM

ENV GO111MODULE=on \
    CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=${TARGETARCH}

COPY . .

RUN go mod download
RUN go build -o out ./main.go

FROM --platform=${BUILDPLATFORM:-linux/amd64} scratch

WORKDIR /

COPY --from=builder /app/out /dwatch_monitor

#EXPOSE 8080
ENTRYPOINT ["/dwatch_monitor"]