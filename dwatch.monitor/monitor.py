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
    print('Retrieving device')
    device = await client.p115(device_ip)

    while True:
        print('Retrieving info')
        resp = serverUp()

        if resp == False:
            print('Oh no, server is not responding')
            await resetDevice(device)
        else:
            print('Very good, device is on')

        print('Sleepy times')

        await asyncio.sleep(60 * check_time)

async def resetDevice(device):
    print("Resetting device")

    info = await device.get_device_info()

    if info.device_on == True:
        print("Device is on, turning off")
        await device.off()

        print("Wait 5 secs")
        await asyncio.sleep(5)

    print("Device is off, turning on")
    await device.on()


def serverUp():
    print('On request')

    try:
        response = requests.get(url)
        if not response.status_code == 200:
            print(f'Got error from server: {response.to_dict()}')
            return False
        print('Got resp')
        return response.text == 'OK'
    except:
        print("Got exception invoking server")
        return False
    

if __name__ == "__main__":
    asyncio.run(main())