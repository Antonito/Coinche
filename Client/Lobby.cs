﻿using System;
using System.Threading;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Coinche.Client
{
    public static class Lobby
    {
        private static readonly string _infos = "LobbyInfo";
        private static readonly string _err = "LobbyError";
        private static readonly string _select = "LobbySelect";
        public static bool IsGameStarted;

        public static void Register(Connection connection)
        {
            connection.AppendIncomingPacketHandler<string>(_infos, LobbyInfoHandler);
            connection.AppendIncomingPacketHandler<string>(_err, LobbyErrorHandler);
            connection.AppendIncomingPacketHandler<string>(_select, LobbySelectHandler);
        }

        public static void Unregister(Connection connection)
        {
            connection.RemoveIncomingPacketHandler(_err);
            connection.RemoveIncomingPacketHandler(_infos);
            connection.RemoveIncomingPacketHandler(_select);
        }

        public static void Connect(Connection connection)
        {
            Console.WriteLine("Which lobby do you want to join ?");
            bool success = false;
            string msg;
            do
            {
                success = Reader.TryReadLine(out msg, 100);
            } while (!success);
            connection.SendObject("SelectLobby", msg);
        }

        private static void LobbySelectHandler(PacketHeader header, Connection connection, string message)
        {
            NetworkGame.Unregister(connection);
            connection.SendObject("LobbyRoomQuit");
            LobbyRoom.Unregister(connection);
            Program.clientInfos.IsRun = false;
            Connect(connection);
        }

        private static void LobbyErrorHandler(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);

            Connect(connection);
        }

        private static void LobbyInfoHandler(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine(message);
            NetworkGame.Register(connection);
            LobbyRoom.Register(connection);

            Program.clientInfos.IsRun = true;
            IsGameStarted = false;
            Console.WriteLine("Send message to lobby: ");
            while (Program.clientInfos.IsRun)
            {
                string msg;
                bool success = Reader.TryReadLine(out msg, 100);
                if (!success)
                    continue;
                else
                {
                    Console.WriteLine("Send message to lobby: ");
                }

                if (msg.StartsWith("/quit"))
                {
                    Program.clientInfos.IsRun = false;
                }
                else if (msg.StartsWith("/ready"))
                {
                    NetworkGame.SendReady(connection);
                    IsGameStarted = true;
                    Program.clientInfos.IsRun = false;
                }
                else
                {
                    connection.SendObject("LobbyRoomMessage", msg);
                }
            }
            if (!IsGameStarted)
            {
                NetworkGame.Unregister(connection);
                connection.SendObject("LobbyRoomQuit");
                LobbyRoom.Unregister(connection);
            }
        }
    }
}
