import asyncio
import os
import requests

from tapo import ApiClient

url = os.environ.get('HEALTHCHECK_URL', 'http://dwatch-server:8080')
device_ip = os.environ.get('DEVICE_IP', '10.1.1.224')
user = os.environ.get('USERNAME', '')
password = os.environ.get('PASSWORD', '')
check_time = os.environ.get('CHECK_TIME', 1)

async def main():
    client = ApiClient(user, password)
    log('Retrieving device')
    device = await client.p115(device_ip)

    while True:
        log('Retrieving info')
        resp = serverUp()

        if resp == False:
            log('Oh no, server is not responding')
            await resetDevice(device)
        else:
            log('Very good, device is on')

        log('Sleepy times')

        await asyncio.sleep(60 * check_time)

async def resetDevice(device):
    log("Resetting device")

    info = await device.get_device_info()

    if info.device_on == True:
        log("Device is on, turning off")
        await device.off()

        log("Wait 5 secs")
        await asyncio.sleep(5)

    log("Device is off, turning on")
    await device.on()


def serverUp():
    log('On request')

    try:
        response = requests.get(url)
        if not response.status_code == 200:
            log(f'Got error from server: {response.to_dict()}')
            return False
        log('Got resp')
        return response.text == 'OK'
    except:
        log("Got exception invoking server")
        return False
    

def log(message):
    print(message, flush=True)

if __name__ == "__main__":
    log('Heeeey')
    asyncio.run(main())