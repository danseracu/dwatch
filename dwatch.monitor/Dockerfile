
FROM --platform=${BUILDPLATFORM:-linux/amd64} docker.io/python:3.11 AS build
ARG BUILDPLATFORM

WORKDIR "/app"

ADD requirements.txt .

ENV PYTHONUNBUFFERED=1

# Install dependecies
# hadolint ignore=DL3008,DL3013
RUN set -eux && \
    apt-get update; \
    apt-get install --no-install-recommends -y \
        python3-dev build-essential patchelf; \
    apt-get clean; \
    rm -rf /var/lib/apt/lists/*; \
    python -m pip install --no-cache-dir --upgrade --force --ignore-installed pip; \
    python -m pip install --no-cache-dir --upgrade -r requirements.txt; \
    python -m pip install --no-cache-dir --upgrade wheel staticx pyinstaller;

ADD monitor.py .

RUN set -eux && \
    pyinstaller --name hw-compiled --onefile monitor.py --paths "$(python -m site --user-site)"; \
    staticx --strip dist/hw-compiled dist/hw; \
    strip -s -R .comment -R .gnu.version --strip-unneeded dist/hw; \
    rm dist/hw-compiled; \
    mkdir -p dist/tmp

FROM --platform=${BUILDPLATFORM:-linux/amd64} scratch
COPY --from=build /app/dist/ /

ENV PYTHONUNBUFFERED=1

ENTRYPOINT ["/hw"]