using System;
using System.Linq;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections;
using Coinche.Common.PacketType;
using NetworkCommsDotNet;
using ProtoBuf;
using System.IO;
using Coinche.Common.Core.Game;
using System.Threading;

namespace Coinche.Client
{
    using Promise = Common.Core.Contract.Promise;

    public static class NetworkGame
    {
        private static readonly string _type = "NetWorkGame";
        private static readonly string _newGame = "NewGame";
        private static readonly string _getCard = "PlayerGetGameCard";
        private static readonly string _selectLobby = "SelectLobbyNetwork";
        private static readonly string _chooseContract = "ChooseContract";
        private static readonly string _chooseContractInfo = "ChooseContractInfo";
        private static readonly string _giveCard = "GiveMeCard";
        private static readonly string _invalidCard = "InvalidCard";
        private static readonly string _endFold = "EndFold";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<byte[]>(_type, InfoGameHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_newGame, NewGameHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_getCard, GetCardHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_selectLobby, SelectLobbyHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_chooseContract, ChooseContractHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_chooseContractInfo, ChooseContractInfoHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_giveCard, GiveCardHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_invalidCard, InvalidCardHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_endFold, EndFoldHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
            connection.RemoveIncomingPacketHandler(_newGame);
            connection.RemoveIncomingPacketHandler(_getCard);
            connection.RemoveIncomingPacketHandler(_selectLobby);
            connection.RemoveIncomingPacketHandler(_chooseContract);
            connection.RemoveIncomingPacketHandler(_chooseContractInfo);
            connection.RemoveIncomingPacketHandler(_giveCard);
            connection.RemoveIncomingPacketHandler(_invalidCard);
            connection.RemoveIncomingPacketHandler(_endFold);
        }

        private static void InfoGameHandler(PacketHeader header, Connection connection, byte[] info)
        {
        }

        private static void NewGameHandler(PacketHeader header, Connection connection, byte[] info)
        {
            Program.clientInfos.ResetCards();
            Console.WriteLine("Starting new game");
            connection.SendObject("NewGameOK");
        }

        private static void EndFoldHandler(PacketHeader header, Connection connection, byte[] info)
        {
            using (var stream = new MemoryStream(info))
            {
                var res = Serializer.Deserialize<EndRound>(stream);
                Console.WriteLine("Winner: " + res.WinnerTeam +
                                  " (" + res.WinnerPoint + ") | Loser: " +
                                  res.LoserPoint);
            }
        }

        private static void InvalidCardHandler(PacketHeader header, Connection connection, byte[] info)
        {
            Console.WriteLine("You cannot play this card.");
            Program.clientInfos.RevertPlay();   
        }

        private static void GiveCardHandler(PacketHeader header, Connection connection, byte[] info)
        {
            int choice = AskUser.AskCard(Program.clientInfos.GetCards());

            if (!Lobby.IsGameStarted)
            {
                return;
            }

            using (var stream = new MemoryStream())
            {
                PlayCard card = new PlayCard
                {
                    CardValue = Program.clientInfos.GetCardType(choice),
                    CardColor = Program.clientInfos.GetCardColor(choice)
                };
                Program.clientInfos.PlayCard(choice);
                Serializer.Serialize(stream, card);
                connection.SendObject("GiveCard", stream.ToArray());
                Console.WriteLine("Card sent !");
            }
        }

        private static void ChooseContractInfoHandler(PacketHeader header, Connection connection, byte[] info)
        {
            using (MemoryStream stream = new MemoryStream(info))
            {
                var contract = Serializer.Deserialize<ContractInfo>(stream);
                Console.WriteLine("Player " + contract.Pseudo + " chose " +
                                  contract.Promise.ToString() + " | " + contract.GameMode.ToString());
            }
        }

        private static void ChooseContractHandler(PacketHeader header, Connection connection, byte[] info)
        {
            ContractRequest contract;
            using (MemoryStream stream = new MemoryStream(info))
            {
                contract = Serializer.Deserialize<ContractRequest>(stream);
            }

            Promise promise = AskUser.AskPromise(contract);
            GameMode gameMode = GameMode.ClassicClover;

            if (promise != Promise.Passe && promise != Promise.Coinche
                && promise != Promise.ReCoinche)
            {
                gameMode = AskUser.AskGameMode();
            }
           
            if (!Lobby.IsGameStarted)
            {
                return;
            }
            using (MemoryStream streamResp = new MemoryStream())
            {
                ContractResponse resp = new ContractResponse
                {
                    Promise = promise,
                    GameMode = gameMode
                };
                Serializer.Serialize(streamResp, resp);
                connection.SendObject("ChooseContractResp", streamResp.ToArray());
                Console.WriteLine("Response sent !");
            }
        }

        // TODO: rm ???
        private static void SelectLobbyHandler(PacketHeader header, Connection connection, byte[] info)
        {
            Console.WriteLine("Contract: ");
            var contract = Console.ReadLine();
        }

        private static void GetCardHandler(PacketHeader header, Connection connection, byte[] info)
        {
            Console.WriteLine("Receiving card...");

            using (MemoryStream stream = new MemoryStream(info))
            {
                var card = Serializer.Deserialize<PlayCard>(stream);
                Console.WriteLine("Got: " + card.CardValue + " | " + card.CardColor);
                Program.clientInfos.AddCard(card);
            }
        }

        public static void SendReady(Connection connection)
        {
            Console.WriteLine("sending ready to server");
            StartGame game = new StartGame
            {
                IsReady = true
            };
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, game);
                byte[] t = stream.ToArray();
                connection.SendObject(_type, t);
            }
        }
    }
}
