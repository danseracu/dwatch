FROM python:alpine as deps

WORKDIR /app

ADD requirements.txt .
RUN python -m pip install -r requirements.txt

FROM deps as run

WORKDIR /app

ADD monitor.py .

CMD ["python", "-u", "./monitor.py"] 