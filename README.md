# DWatch

DWatch is a very simple distributed watchdog. It monitors a server's health, and, in case of failure, resets a smart plug, so as to power cycle the server

## Description

After finally perfecting my home network setup, i realized just as i left for vacation one major flaw. In case of a kernel panic or any other os freeze, i am stuck until i get physical access back to the server to reset it. So the idea for DWatch was born. DWatch has 2 components: a basic healthcheck server, and a monitor. The monitor periodically queries the server, and in case of failure, restarts a TP-Link Tapo p115 smart plug (api should be open for more). The server is optional, since any kind of healthcheck can be used

## Running

For a quick local setup, run: ``` docker-compose up -d ``` from the root of the project. This will not be an ideal setup however, since both components are on the same machine

### DWatch.Server

A simple http server, that returns OK when invoked. No configurations are available.
This component can be replaced with the healthcheck of your choice. Running:

```
docker run -d -p 8080:8080 dwatch/server
```

### DWatch.Monitor

The real magic is here. The monitor continuosly pings the server/healtcheck endpoint, and in case of an unexpected result, timeout or any other exception, will attempt to perform a restart of the device, by restarting the plug using the [Python Tapo API](https://github.com/mihai-dinculescu/tapo/tree/main).
Running:
```
docker run -d -e USERNAME="t user -e PASSWORD="t_pass" -e HEALTHCHECK_URL="http://dwatch.server:8080" dwatch/monitor
```

Configurations are done via environment variables:

| Environment Variable Name     | Description                                                      |
| ----------------------------- | :--------------------------------------------------------------: |
| USERNAME *required*           | Username used to login to the Tapo device                        |
| PASSWORD *required*           | Password used to login to the Tapo device                        |
| HEALTHCHECK_URL               | Contains the url that will be invoked to check for server health |
| DEVICE_IP                     | IP address of the Tapo device                                    |
| CHECK_TIME                    | Time in minutes to wait between checks                           |


## Credits

Using [Tapo API](https://github.com/mihai-dinculescu/tapo)
