using System;
using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    public class StartRound
    {
        [ProtoMember(1)]
        public bool IsReady { get; set; }

    }
}
