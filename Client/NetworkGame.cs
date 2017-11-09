using System;
using NetworkCommsDotNet.Connections;
using Coinche.Common.PacketType;
using NetworkCommsDotNet;
using ProtoBuf;
using System.IO;

namespace Coinche.Client
{
    using Promise = Common.Core.Contract.Promise;

    public static class NetworkGame
    {
        private static readonly string _type = "NetWorkGame";
        private static readonly string _getCard = "PlayerGetGameCard";
        private static readonly string _selectLobby = "SelectLobbyNetwork";
        private static readonly string _chooseContract = "ChooseContract";
        private static readonly string _chooseContractInfo = "ChooseContractInfo";

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<byte[]>(_type, InfoGameHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_getCard, GetCardHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_selectLobby, SelectLobbyHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_chooseContract, ChooseContractHandler);
            connection.AppendIncomingPacketHandler<byte[]>(_chooseContractInfo, ChooseContractInfoHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_type);
            connection.RemoveIncomingPacketHandler(_getCard);
            connection.RemoveIncomingPacketHandler(_selectLobby);
            connection.RemoveIncomingPacketHandler(_chooseContract);
            connection.RemoveIncomingPacketHandler(_chooseContractInfo);
        }

        private static void InfoGameHandler(PacketHeader header, Connection connection, byte[] info)
        {
        }

        private static void ChooseContractInfoHandler(PacketHeader header, Connection connection, byte[] info)
        {
            MemoryStream stream = new MemoryStream(info);
            var contract = Serializer.Deserialize<ContractInfo>(stream);
            Console.WriteLine("Player " + contract.Pseudo + " chose " + 
                              contract.Promise + " | " + contract.Color);
        }

        private static void ChooseContractHandler(PacketHeader header, Connection connection, byte[] info)
        {
            MemoryStream stream = new MemoryStream(info);
            var contract = Serializer.Deserialize<ContractRequest>(stream);
            foreach (Promise e in Enum.GetValues(typeof(Promise)))
            {
                if (e == 0 || e >= contract.MinimumValue)
                {
                    Console.WriteLine(e.ToString() + " -> " + (int)e);
                }
            }

            // TODO
            Console.WriteLine("Contract: ");

            var promise = Promise.Points150;
            MemoryStream streamResp = new MemoryStream();
            ContractResponse resp = new ContractResponse
            {
                Promise = promise,
                Color = Common.Core.Cards.CardColor.Clover
            };
            Serializer.Serialize(streamResp, resp);
            connection.SendObject("ChooseContractResp", streamResp.ToArray());
            Console.WriteLine("Response sent !");
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

            MemoryStream stream = new MemoryStream(info);
            var card = Serializer.Deserialize<PlayCard>(stream);
            Console.WriteLine("Got: " + card.CardValue + " | " + card.CardColor);

        }

        public static void SendReady(Connection connection)
        {
            Console.WriteLine("sending ready to server");
            StartGame game = new StartGame
            {
                IsReady = true
            };
            //TODO: set the Stream into a 'client' class like in server
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize(stream, game);
            byte[] t = stream.ToArray();
            connection.SendObject(_type, t);
        }
    }
}
