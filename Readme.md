## Description

The application provides an api for playing sea battle for one of the players.

## Configuration:

The application can use `Postgre` storage (default) or in-memory storage. If you want to use in-memory storage, use the `StorageConfiguration__UseMemoryStorage=true` environment variable like this:

```shell
docker build -t seabattle .
docker run --name game -d -p 20270:20270 -e StorageConfiguration__UseMemoryStorage=true seabattle

#cleanup
docker rm seabattle -f
```

If you want to use a database then use the docker compose file

```shell
docker-compose up -d

#cleanup
docker-compose down
```

After build, the application will be available at localhost: 20270 and you can make requests, for example:

```http
POST /ship
Host: localhost:20270
Content-Type:  application/json

{
  "coordinates": "3B 3b"
}
```

## Rules

1. Only one game is available at a time, until you complete it the next game cannot be started
2. Ships can be any rectangular shape and must not intersect or go beyond the edge of the field.
3. You can not create ships twice
4. You can not place ships if the game is not created
5. You cannot place ships if the game is over.
6. You cannot shoot twice at the same point in the same game.
7. You are not allowed to shoot outside the field
8. You are not allowed to shoot if the game is over.

## Examples of requests

### Create a new game

```http
POST /create-matrix
Content-Type:  application/json

{
  "range": 5
}
```

`range` - field size (square)

### Populate ships

```http
POST /ship
Content-Type:  application/json

{
  "coordinates": "3B 3b, 55W 9F, 901A 4T"
}
```

`coordinates` - coordinates of ships to be located on the playing field

### Shot

```http
POST /shot
Content-Type:  application/json

{
    "coord": " 3b"
}
```

`coord` - shot coordinates

### Game statistic 

```http
GET /state
Content-Type:  application/json
```

### End game

```http
POST /clear
Content-Type: application/json
```

