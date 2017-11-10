using System;
using System.Linq;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections;
using Coinche.Common.PacketType;
using NetworkCommsDotNet;
using ProtoBuf;
using System.IO;
using Coinche.Common.Core.Game;

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

        private static List<Card> _cards;
        private static List<Card> _cardsPlayed;

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
            _cards = new List<Card>();
            _cardsPlayed = new List<Card>();
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
            _cards.Add(_cardsPlayed.Last());
            _cardsPlayed.Remove(_cardsPlayed.Last());
        }

        private static void GiveCardHandler(PacketHeader header, Connection connection, byte[] info)
        {
            Console.WriteLine("Number of cards: " + _cards.Count);
            for (var i = 0; i < _cards.Count; ++i)
            {
                Console.WriteLine(i + ") " + _cards[i].Value.ToString() + " - " + _cards[i].Color.ToString());
            }
            bool valid = false;
            int choice = 0;
            do
            {
                try
                {
                    Console.WriteLine("Please select a card: ");
                    var cardChoice = Console.ReadLine();
                    choice = int.Parse(cardChoice);
                    if (choice < 0 || choice >= _cards.Count)
                    {
                        throw new IndexOutOfRangeException("Invalid input");
                    }
                    valid = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input.");
                }
            } while (!valid);


            using (var stream = new MemoryStream())
            {
                PlayCard card = new PlayCard
                {
                    CardValue = _cards[choice].Value,
                    CardColor = _cards[choice].Color
                };
                _cardsPlayed.Add(_cards[choice]);
                _cards.RemoveAt(choice);
                Serializer.Serialize(stream, card);
                connection.SendObject("GiveCard", stream.ToArray());
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

            Promise promise = Promise.Points150;
            GameMode gameMode = Common.Core.Game.GameMode.ClassicClover;

            Console.WriteLine("Choose between the following promise:");
            foreach (Promise e in Enum.GetValues(typeof(Promise)))
            {
                if (e == Promise.Passe || e >= contract.MinimumValue)
                {
                    if (e == Promise.General)
                    {
                        Console.WriteLine((int)e);
                    }
                    else
                    {
                        Console.Write((int)e + ", ");

                    }
                }
            }

            Console.Write(">");
            // clearing the input buffer before asking something to user
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
            bool success = false;
            while (!success)
            {
                success = Reader.TryReadLine(out string userInput, 100);
                if (success)
                {
                    if (Enum.IsDefined(typeof(Promise), Int32.Parse(userInput)))
                    {
                        promise = ((Promise)Int32.Parse(userInput));
                    }
                    else
                    {
                        Console.WriteLine("Wrong choice\n>");
                        success = false;
                    }
                }
            }

            {
                Console.WriteLine("Choose between the following game mode:");
                int menuCount = 0;
                foreach (GameMode e in Enum.GetValues(typeof(GameMode)))
                {
                    Console.WriteLine(menuCount + ") " + e.ToString());
                    menuCount++;
                }

                Console.Write(">");
                success = false;
                do
                {
                    success = Reader.TryReadLine(out string userInput, 100);
                    if (success)
                    {
                        if (Enum.IsDefined(typeof(GameMode), Int32.Parse(userInput)))
                        {
                            gameMode = ((GameMode)Int32.Parse(userInput));
                        }
                        else
                        {
                            Console.WriteLine("Wrong choice\n>");
                            success = false;
                        }
                    }
                } while (!success);
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
                _cards.Add(new Card(card.CardValue, card.CardColor));
            }

        }

        public static void SendReady(Connection connection)
        {
            Console.WriteLine("sending ready to server");
            StartGame game = new StartGame
            {
                IsReady = true
            };
            //TODO: set the Stream into a 'client' class like in server
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, game);
                byte[] t = stream.ToArray();
                connection.SendObject(_type, t);
            }
        }
    }
}
