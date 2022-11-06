# emojivoto-dotnet

This repository contains a `.NET Core` ported version of the [EmojiVoto](https://github.com/BuoyantIO/emojivoto) project, which is written in Go. It also showcases `Opentelemetry`'s benefits through multiple scenarios using `docker-compose` and `kubernetes`.

## Build docker images

```powershell
docker-compose --profile app build
```

## Different monitoring scenarios

### No monitoring, just console logging

Without passing extra parameters/environment vars, this setup does only basic console logging.
The profile `app` is passed to only deploy the services of the app itself.

```powershell
docker-compose --profile app up -d --remove-orphans
```

With this setup we have an application that is up and running at [localhost:8080](http://localhost:8080),
but without any means of observability, except that console output.

```mermaid
graph TD;
    emojiui-->|logs|console;
    emojiui-->|traces|?;
    emojiui-->|metrics|?;
    emojisvc-->|logs|console;
    emojisvc-->|traces|?;
    emojisvc-->|metrics|?;
    votingsvc-->|logs|console;
    votingsvc-->|traces|?;
    votingsvc-->|metrics|?;
    votebot-->|logs|console;
    votebot-->|traces|?;
    votebot-->|metrics|?;
```

To bring the app back down

```powershell
docker-compose down
```

### Monitoring to individual services (jaeger, seq, prometheus)

Providing extra environment variables through `docker-compose.individual.yaml`, the app can be reconfigured to start outputting to the individual services `seq`, `jaeger` and `prometheus`.

```mermaid
graph TD;
    emojiui-->|logs|seq;
    emojiui-->|traces|jaeger;
    emojiui-->|metrics|prometheus;
    emojisvc-->|logs|seq;
    emojisvc-->|traces|jaeger;
    emojisvc-->|metrics|prometheus;
    votingsvc-->|logs|seq;
    votingsvc-->|traces|jaeger;
    votingsvc-->|metrics|prometheus;
    votebot-->|logs|seq;
    votebot-->|traces|jaeger;
    votebot-->|metrics|prometheus;
```

```powershell
docker-compose --profile app --profile individual -f docker-compose.yaml -f docker-compose.individual.yaml up -d --remove-orphans
```

Each component is reconfigured to output to each monitoring service. That means each service now outputs:

* metrics on an endpoint which is then scraped by `prometheus`
* traces to `jaeger`
* logs directly to `seq`

Although we have some observability now, we still need to reconfigure each service.

### Monitoring to individual services (loki, tempo, prometheus)

We could also reconfigure to output to other services like `loki`, `tempo` and `prometheus`, but we need to do that by adjusting each component individually.

```mermaid
graph TD;
    emojiui-->|logs|loki;
    emojiui-->|traces|tempo;
    emojiui-->|metrics|prometheus;
    emojisvc-->|logs|loki;
    emojisvc-->|traces|tempo;
    emojisvc-->|metrics|prometheus;
    votingsvc-->|logs|loki;
    votingsvc-->|traces|tempo;
    votingsvc-->|metrics|prometheus;
    votebot-->|logs|loki;
    votebot-->|traces|tempo;
    votebot-->|metrics|prometheus;
```

```powershell
docker-compose --profile app --profile grafana -f docker-compose.yaml -f docker-compose.individual-grafana.yaml up -d --remove-orphans
```

Each component is reconfigured to output to each monitoring service. That means each service outputs:

* metrics on an endpoint which is scraped by `prometheus`
* traces to `tempo`
* logs directly to `loki`

Downside is still that we heave to reconfigure each service to get monitoring up and running.

### Monitoring through opentelemetry (grafana backend)

```mermaid
graph TD;
    emojiui-->|logs|opentelemetry-collector;
    emojiui-->|traces|opentelemetry-collector;
    emojiui-->|metrics|opentelemetry-collector;
    emojisvc-->|logs|opentelemetry-collector;
    emojisvc-->|traces|opentelemetry-collector;
    emojisvc-->|metrics|opentelemetry-collector;
    votingsvc-->|logs|opentelemetry-collector;
    votingsvc-->|traces|opentelemetry-collector;
    votingsvc-->|metrics|opentelemetry-collector;
    votebot-->|logs|opentelemetry-collector;
    votebot-->|traces|opentelemetry-collector;
    votebot-->|metrics|opentelemetry-collector;
    opentelemetry-collector-->|logs|loki
    opentelemetry-collector-->|traces|tempo
    opentelemetry-collector-->|metrics|prometheus
```

We let the app output to just one component: `opentelemetry-collector`. From there we then can decide where each output should go without
having to reconfigure it in the application itself. Instead of configuring three endpoints, we just have to deal with one.

Reconfiguring to output to opentelemetry

```powershell
docker-compose --profile app --profile otlp -f docker-compose.yaml -f docker-compose.otlp.yaml up -d --remove-orphans
```

### Monitoring through opentelemetry (datadog)

Now instead having to run that monitoring backend outselves, we can also choose to output to a cloud service like [Datadog](https://datadoghq.com) or [Splunk](https://www.splunk.com/). This example shows exporting to Datadog.

> Make sure you create an `.env` file with your `DD_API_KEY` and `DD_SITE` values.

```mermaid
graph TD;
    emojiui-->|logs|opentelemetry-collector;
    emojiui-->|traces|opentelemetry-collector;
    emojiui-->|metrics|opentelemetry-collector;
    emojisvc-->|logs|opentelemetry-collector;
    emojisvc-->|traces|opentelemetry-collector;
    emojisvc-->|metrics|opentelemetry-collector;
    votingsvc-->|logs|opentelemetry-collector;
    votingsvc-->|traces|opentelemetry-collector;
    votingsvc-->|metrics|opentelemetry-collector;
    votebot-->|logs|opentelemetry-collector;
    votebot-->|traces|opentelemetry-collector;
    votebot-->|metrics|opentelemetry-collector;
    opentelemetry-collector-->|logs|datadog
    opentelemetry-collector-->|traces|datadog
    opentelemetry-collector-->|metrics|datadog
```

```powershell
docker-compose --profile app --profile datadog -f docker-compose.yaml -f docker-compose.otlp-datadog.yaml up -d --remove-orphans
```
