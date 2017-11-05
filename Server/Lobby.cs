﻿using System;
using System.Linq;
using System.Collections.Generic;
using NetworkCommsDotNet.Connections;
using Coinche.Server.Core;

namespace Coinche.Server
{
    public class Lobby
    {
        private readonly string _name;
        private readonly List<Connection> _connections;

        public string Name { get { return _name; } }
        public int NbPlayers { get { return _connections.Count(); } }

        // TODO: Improve with somethign using yield ?
        public List<Connection> Connection { get { return _connections; }}

        public Lobby(string name)
        {
            _name = name;
            _connections = new List<Connection>();
        }

        public void AddPlayer(Connection connection)
        {
            if (IsFull())
            {
                throw new Exception.LobbyError("Lobby is full.");
            }
            if (IsInLobby(connection)) 
            {
                throw new Exception.LobbyError("Client is already in lobby.");
            }
            _connections.Add(connection);
        }

        public bool IsInLobby(Connection connection)
        {
            return _connections.Contains(connection);
        }

        public void RemovePlayer(Connection connection)
        {
            _connections.Remove(connection);
        }

        private bool IsFull()
        {
            return _connections.Count() == 4;
        }
    }
}
