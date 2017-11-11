#!/usr/bin sh

if [ "$#" -ne 2 ]; then
    echo "Usage: $0 executable address:port"
    exit 1
fi

EXEC=$1
ADDR=$2
PSEUDO=$(openssl rand -hex 12)
LOBBY=toto

echo -ne "$ADDR\n$PSEUDO\n$LOBBY\nHello guys !\n/ready\n" | mono $EXEC 
exit 0