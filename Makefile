.PHONY: build run test clean

build:
	docker compose build

run:
	docker compose up

stop:
	docker compose down

clean:
	docker compose down -v
	
test:
	dotnet test

install:
	dotnet restore