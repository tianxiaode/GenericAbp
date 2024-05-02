#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p 4fdfc5b4-1f77-4e3c-bc4e-77351a8462a1 -t
    fi
    cd ../
fi

docker-compose up -d
