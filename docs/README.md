# Documentation

This document describes our network protocol.

We use *Networkcomms* as a networking library, and *Protobuf-net* as our serialization library.

## Network Protocol

The connection to a game can be done like this :

1. After a client has established a connection, the server welcomes it with a message (*string*).
2. The client sends its pseudodym
3. The client select a lobby. 
 	- If the lobby is full, the client will have to choose another lobby
	- Otherwise
		- If the lobby does not exists, it will be created, then joined.
		- If the lobby has space, it will be joined.
4. The client can now chat with other players that are in the same lobby
	- The client can send a "LobbyRoomQuit" event to the server, which will allow the player to choose another lobby.
	- When ready, the client can send a "StartGame" packet, notifying the server that the player is ready.
5. When all 4 players are ready, the server starts the game.
	- From now on, if a player disconnects, the full lobby will be disconnected.
6. Then, the server will ask each player individually to select a contract.
	- If every player select a "Pass" contract, a new game will be started.
	- The contract selection stops when 3 players select a "Pass" contract.
	- During this step, the server sends a *ContractRequest* packet to a client, the client responds with a *ContractResponse* packet, and the server notifies every other player by sending a *ContractInfo* packet.
7. Once the contract is chosen, the server asks every player to play a card.
	- Card verification are done on server-side
	- If a player plays an invalid card, the server sends an "InvalidCard" event to the client, and asks for another card.
	- During this step, the server sends a "GiveMeCard" event, the player then responds with a *PlayCard* packet.
8. When the fold is over (when no one has no card left), the server notifies everyone with a *EndRound* packet.
9. Back to step 6, until a team wins.
	- When a team wins, all players are notified with a "MatchWinner" packet.

## Packets

StartGame:

```c#
   [ProtoContract]
    public class StartGame
    {
        [ProtoMember(1)]
        public bool IsReady { get; set; }

    }
```

Contract:

```c#
    [ProtoContract]
    public sealed class ContractResponse
    {
        [ProtoMember(1)]
        public Core.Contract.Promise Promise { get; set; }
        [ProtoMember(2)]
        public Core.Game.GameMode GameMode { get; set; }
    }

    [ProtoContract]
    public sealed class ContractRequest
    {
        [ProtoMember(1)]
        public Core.Contract.Promise MinimumValue { get; set; }
    }

    [ProtoContract]
    public sealed class ContractInfo
    {
        [ProtoMember(1)]
        public Core.Contract.Promise Promise { get; set; }
        [ProtoMember(2)]
        public Core.Game.GameMode GameMode { get; set; }
        [ProtoMember(3)]
        public string Pseudo { get; set; }
    }
```

PlayCard:

```c#
    [ProtoContract]
    public class PlayCard
    {
        [ProtoMember(1)]
        public Core.Cards.CardType CardValue { get; set; }
        [ProtoMember(2)]
        public Core.Cards.CardColor CardColor { get; set; }
    }
```

EndRound:

```c#
    [ProtoContract]
    public class EndRound
    {
        [ProtoMember(1)]
        public int WinnerTeam { get; set; }
        [ProtoMember(2)]
        public int WinnerPoint { get; set; }
        [ProtoMember(3)]
        public int LoserPoint { get; set; }
    }
```

MatchWinner:

```c#
    [ProtoContract]
    public class MatchWinner
    {
        [ProtoMember(1)]
        public int TeamWinner { get; set; }
        [ProtoMember(2)]
        public string PseudoA { get; set; }
        [ProtoMember(3)]
        public string PseudoB { get; set; }
    }
```

## Code documentation
Code documentation is provided in form of XML documentation, directly in the source files.