
FROM --platform=${BUILDPLATFORM:-linux/amd64} debian:latest AS build
ARG BUILDPLATFORM

WORKDIR "/app"

ADD requirements.txt .

ENV PYTHONUNBUFFERED=1

# Install dependecies
# hadolint ignore=DL3008,DL3013
RUN set -eux && apt-get update; 

#Install python

RUN  apt-get install --no-install-recommends -y \
        python3-pip python3-dev python3-setuptools python3-wheel python3-apt python3.11-venv \
        build-essential patchelf; \
        rm -rf /var/lib/apt/lists/*; \
        python3 -m venv /venv;

RUN rm -f /usr/lib/python3.11/EXTERNALLY-MANAGED
ENV PATH="/opt/venv/bin:$PATH"

# RUN ls /venv/bin;
# RUN . /venv/bin/activate;



RUN  apt-get clean; \
    rm -rf /var/lib/apt/lists/*; \
    python3 -m pip install --upgrade pip; \
    python3 -m pip install -r requirements.txt; \
    python3 -m pip install --upgrade wheel staticx pyinstaller;

ADD monitor.py .

RUN set -eux && \
    pyinstaller --name hw-compiled --onefile monitor.py --paths "$(python3 -m site --user-site)"; \
    staticx --strip dist/hw-compiled dist/hw; \
    strip -s -R .comment -R .gnu.version --strip-unneeded dist/hw; \
    rm dist/hw-compiled; \
    mkdir -p dist/tmp

FROM --platform=${BUILDPLATFORM:-linux/amd64} scratch
COPY --from=build /app/dist/ /

ENV PYTHONUNBUFFERED=1

ENTRYPOINT ["/hw"]