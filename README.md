# EmojiVoto-dotnet

## Description

This repository contains a `.NET Core` ported version of the [EmojiVoto](https://github.com/BuoyantIO/emojivoto) project (written in Go). It also showcases `Opentelemetry`'s benefits through multiple scenarios using `docker-compose` and `kubernetes`.

```mermaid
graph LR;
  subgraph EmojiVoto
  VoteBot --REST--> EmojiUI;
  EmojiUI --gRPC--> EmojiSvc;
  EmojiUI --gRPC--> EmojiVoting;
  end
  style VoteBot fill:#fcba03,color:#000
  style EmojiUI fill:#03fc28,color:#000
  style EmojiSvc fill:#03fc28,color:#000
  style EmojiVoting fill:#03fc28,color:#000
```

## Build docker images

Have your docker engine running. Then run:

```powershell
.\build.ps1 --target Docker-Build
```

## Different monitoring scenarios

### No monitoring, just console logging

Without passing extra parameters/environment vars, this setup does only basic console logging. This is the use case of many applications today that don't have any monitoring in place and just for file logging. You forces you to go fetch the logs from the servers where the application is running and spitting through endless lines of logs to find a clue of what is going on. In multithreaded apps that's even harder as you get the log lines ordered by timestamp, so threads get merged together in the log files.

The profile `app` is passed to only deploy the services of the app itself.

```powershell
docker-compose --profile app up -d --remove-orphans
```

With this setup we have an application that is up and running at [localhost:8080](http://localhost:8080),
but without any means of observability, except that console output.

```mermaid
graph TD;
  subgraph EmojiVoto
    VoteBot --REST--> EmojiUI
    EmojiUI --gRPC--> EmojiSvc
    EmojiUI --gRPC--> EmojiVoting
  end
  subgraph Monitoring
    Console
  end
  EmojiUI -.logs.-> Console;
  EmojiSvc -.logs.-> Console;
  EmojiVoting -.logs.-> Console;
  VoteBot -.logs.-> Console;

  style VoteBot fill:#fcba03,color:#000
  style EmojiUI fill:#03fc28,color:#000
  style EmojiSvc fill:#03fc28,color:#000
  style EmojiVoting fill:#03fc28,color:#000
  style Console fill:#0356fc,color:#000
```

To bring the app back down

```powershell
docker-compose down
```

### Monitoring to individual services (jaeger, seq, prometheus)

Providing extra environment variables through `docker-compose.individual.yaml`, the app can be reconfigured to start outputting to those individual services `seq`, `jaeger` and `prometheus`.

```powershell
docker-compose --profile app --profile individual -f docker-compose.yml -f ./docker-compose/docker-compose.individual.yaml up -d --remove-orphans
```

```mermaid
graph TD;
  subgraph EmojiVoto
    VoteBot --REST--> EmojiUI
    EmojiUI --gRPC--> EmojiSvc
    EmojiUI --gRPC--> EmojiVoting
  end
    subgraph Monitoring
      Seq
      Jaeger
      Prometheus
    end
    EmojiUI -.logs.-> Seq;
    EmojiUI -.traces.-> Jaeger;
    EmojiUI -.metrics.-> Prometheus;
    EmojiSvc -.logs.-> Seq;
    EmojiSvc -.traces.-> Jaeger;
    EmojiSvc -.metrics.-> Prometheus;
    EmojiVoting -.logs.-> Seq;
    EmojiVoting -.traces.-> Jaeger;
    EmojiVoting -.metrics.-> Prometheus;
    VoteBot -.logs.-> Seq;
    VoteBot -.traces.-> Jaeger;
    VoteBot -.metrics.-> Prometheus;

  style VoteBot fill:#fcba03,color:#000
  style EmojiUI fill:#03fc28,color:#000
  style EmojiSvc fill:#03fc28,color:#000
  style EmojiVoting fill:#03fc28,color:#000
  style Seq fill:#0356fc,color:#000
  style Jaeger fill:#0356fc,color:#000
  style Prometheus fill:#0356fc,color:#000
```

Each component is reconfigured to output to each monitoring service. That means each service now outputs:

* metrics on an endpoint which is then scraped by `prometheus` at [http://localhost:9090](http://localhost:9090)
* traces to `jaeger` at [http://localhost:16686](http://localhost:16686)
* logs directly to `seq` at [http://localhost:5341](http://localhost:5341)

Although we have some observability now, we still need to reconfigure each service.

```powershell
docker-compose --profile app --profile individual -f docker-compose.yml -f ./docker-compose/docker-compose.individual.yaml down
```

### Monitoring to individual services (loki, tempo, prometheus)

It would also be nice to see some correlation between those services, like searching for the corresponding log-entries for a given TraceId.
We could reconfigure to output to other services that do just that like `Loki`, `Tempo` and `Prometheus` (made by Grafana), but we need to do that by adjusting each component individually.

```mermaid
graph TD;
  subgraph EmojiVoto
    VoteBot --REST--> EmojiUI
    EmojiUI --gRPC--> EmojiSvc
    EmojiUI --gRPC--> EmojiVoting
  end
    EmojiUI -.logs.-> Loki
    EmojiUI -.traces.-> Tempo
    EmojiUI -.metrics.-> Prometheus
    EmojiSvc -.logs.-> Loki
    EmojiSvc -.traces.-> Tempo
    EmojiSvc -.metrics.-> Prometheus
    EmojiVoting -.logs.-> Loki
    EmojiVoting -.traces.-> Tempo
    EmojiVoting -.metrics.-> Prometheus
    VoteBot -.logs.-> Loki
    VoteBot -.traces.-> Tempo
    VoteBot -.metrics.-> Prometheus
  subgraph one
    Loki <--> Tempo
    Prometheus <--> Loki
    Tempo <--> Prometheus
  end

  style VoteBot fill:#fcba03,color:#000
  style EmojiUI fill:#03fc28,color:#000
  style EmojiSvc fill:#03fc28,color:#000
  style EmojiVoting fill:#03fc28,color:#000
  style Loki fill:#0356fc,color:#000
  style Tempo fill:#0356fc,color:#000
  style Prometheus fill:#0356fc,color:#000
```

```powershell
docker-compose --profile app --profile grafana -f docker-compose.yml -f ./docker-compose/docker-compose.individual-grafana.yaml up -d --remove-orphans
```

Each component is reconfigured to output to each monitoring service. That means each service outputs:

* logs directly to `Loki`
* traces to `Tempo`
* metrics on an endpoint which is scraped by `Prometheus`

All these services can be accessed through Grafana [http://localhost:3000](http://localhost:3000)

Downside is still that we heave to reconfigure each service to get monitoring up and running.

```powershell
docker-compose --profile app --profile grafana -f docker-compose.yml -f ./docker-compose/docker-compose.individual-grafana.yaml down
```

### Monitoring through opentelemetry (grafana backend)

```mermaid
graph TD;
  subgraph EmojiVoto
    VoteBot --REST--> EmojiUI
    EmojiUI --gRPC--> EmojiSvc
    EmojiUI --gRPC--> EmojiVoting
  end
    EmojiUI -.logs.-> OpenTelemetry;
    EmojiUI -.traces.-> OpenTelemetry;
    EmojiUI -.metrics.-> OpenTelemetry;
    EmojiSvc -.logs.-> OpenTelemetry;
    EmojiSvc -.traces.-> OpenTelemetry;
    EmojiSvc -.metrics.-> OpenTelemetry;
    EmojiVoting -.logs.-> OpenTelemetry;
    EmojiVoting -.traces.-> OpenTelemetry
    EmojiVoting -.metrics.-> OpenTelemetry
    VoteBot -.logs.-> OpenTelemetry
    VoteBot -.traces.-> OpenTelemetry
    VoteBot -.metrics.-> OpenTelemetry
  subgraph Monitoring
    OpenTelemetry -.logs.-> Loki
    OpenTelemetry -.traces.-> Tempo
    OpenTelemetry -.metrics.-> Prometheus
    Loki <--> Tempo
    Tempo <--> Prometheus
    Prometheus <--> Loki
  end
  style VoteBot fill:#fcba03,color:#000
  style EmojiUI fill:#03fc28,color:#000
  style EmojiSvc fill:#03fc28,color:#000
  style EmojiVoting fill:#03fc28,color:#000
  style OpenTelemetry fill:#0356fc,color:#000
  style Loki fill:#0356fc,color:#000
  style Tempo fill:#0356fc,color:#000
  style Prometheus fill:#0356fc,color:#000
```

We let the app output to just one component: `opentelemetry-collector`. From there we then can decide where each output should go without
having to reconfigure it in the application itself. Instead of configuring three endpoints, we just have to deal with one.

Reconfiguring to output to opentelemetry

```powershell
docker-compose --profile app --profile otlp -f docker-compose.yml -f ./docker-compose/docker-compose.otlp.yaml up -d --remove-orphans
```

### Monitoring through opentelemetry (datadog)

Now instead having to run that monitoring backend ourselves, we can also choose to output to a cloud service like [Datadog](https://datadoghq.com) or [Splunk](https://www.splunk.com/). This example shows exporting to Datadog.

> Make sure you create an `.env` file with your `DD_API_KEY` and `DD_SITE` values. This setup will then automatically fill in in the required place.

```mermaid
graph TD;
  subgraph EmojiVoto
    VoteBot --REST--> EmojiUI
    EmojiUI --gRPC--> EmojiSvc
    EmojiUI --gRPC--> EmojiVoting
  end
    EmojiUI -.logs.-> OpenTelemetry
    EmojiUI -.traces.-> OpenTelemetry
    EmojiUI -.metrics.-> OpenTelemetry
    EmojiSvc -.logs.-> OpenTelemetry
    EmojiSvc -.traces.-> OpenTelemetry
    EmojiSvc -.metrics.-> OpenTelemetry
    EmojiVoting -.logs.-> OpenTelemetry
    EmojiVoting -.traces.-> OpenTelemetry
    EmojiVoting -.metrics.-> OpenTelemetry
    VoteBot -.logs.-> OpenTelemetry
    VoteBot -.traces.-> OpenTelemetry
    VoteBot -.metrics.-> OpenTelemetry
  subgraph Monitoring
    OpenTelemetry -.logs.-> DataDog
    OpenTelemetry -.traces.-> DataDog
    OpenTelemetry -.metrics.-> DataDog
  end
  style VoteBot fill:#fcba03,color:#000
  style EmojiUI fill:#03fc28,color:#000
  style EmojiSvc fill:#03fc28,color:#000
  style EmojiVoting fill:#03fc28,color:#000
  style DataDog fill:#0356fc,color:#000
  style OpenTelemetry fill:#0356fc,color:#000
```

```powershell
docker-compose --profile app --profile datadog -f docker-compose.yml -f ./docker-compose/docker-compose.otlp-datadog.yaml up -d --remove-orphans
```

To bring it back down

```powershell
docker-compose --profile app --profile datadog -f docker-compose.yml -f ./docker-compose/docker-compose.otlp-datadog.yaml down
```

## Enable metrics from docker engine for docker_stats

Reference: [https://docs.docker.com/config/daemon/prometheus/#configure-docker](https://docs.docker.com/config/daemon/prometheus/#configure-docker)

To configure the Docker daemon as a Prometheus target, you need to specify the metrics-address. The best way to do this is via the daemon.json, which is located at one of the following locations by default. If the file does not exist, create it.

* Linux: `/etc/docker/daemon.json`
* Windows Server: `C:\ProgramData\docker\config\daemon.json`
* Docker Desktop for Mac / Docker Desktop for Windows: Click the Docker icon in the toolbar, select Preferences, then select Daemon. Click Advanced.
If the file is currently empty, paste the following:

```json
{
  "metrics-addr" : "127.0.0.1:9323",
  "experimental" : true
}
```

If the file is not empty, add those two keys, making sure that the resulting file is valid JSON. Be careful that every line ends with a comma (,) except for the last line.

Save the file, or in the case of Docker Desktop for Mac or Docker Desktop for Windows, save the configuration. Restart Docker.

Docker now exposes Prometheus-compatible metrics on port 9323.

> You only need to enable this if you want metrics from the underlying docker daemon, pulled by the opentelemetry-collector.
