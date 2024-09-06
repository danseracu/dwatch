package main

import (
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"strconv"
	"time"

	"github.com/achetronic/tapogo/pkg/tapogo"
)

var quit = make(chan struct{})

var ip = os.Getenv("DEVICE_IP")
var username = os.Getenv("USERNAME")
var password = os.Getenv("PASSWORD")

var url string
var sleepTime int

func main() {
	log.Println("Starting app")

	if val, ok := os.LookupEnv("HEALTHCHECK_URL"); ok {
		url = val
	} else {
		url = "http://localhost:8080"
	}

	if val, ok := os.LookupEnv("CHECK_TIME"); ok {
		sleepTime, _ = strconv.Atoi(val)
	} else {
		sleepTime = 5
	}

	go loop()

	<-quit
}

func reset() {
	log.Println("Resetting device")
	client, err := tapogo.NewTapo(ip, username, password, &tapogo.TapoOptions{})

	if err != nil {
		fmt.Print("Error connecting")
		fmt.Print(err)
		return
	}

	log.Println("Turning off")

	_, err = client.TurnOff()

	if err != nil {
		fmt.Print("Error turning off")
		fmt.Print(err)
		return
	}

	time.Sleep(10 * time.Second)

	log.Println("Turning on")

	_, err = client.TurnOn()

	if err != nil {
		fmt.Print("Error turning on")
		fmt.Print(err)
		return
	}
}

func loop() {
	log.Println("Starting loop")

	for {
		if isServerUp() {
			log.Println("Server is up")
		} else {
			log.Println("Server is not up")
			reset()
		}

		sleep()
	}
}

func sleep() {
	log.Println("Sleeping")

	time.Sleep(time.Duration(sleepTime) * time.Minute)
}

func isServerUp() bool {
	log.Println("Checking server")

	resp, err := http.Get(url)

	if err != nil {
		log.Println(err.Error())
		return false
	}

	responseData, err := io.ReadAll(resp.Body)
	if err != nil {
		log.Fatal(err)
		return false
	}

	return string(responseData) == "OK"
}
