using System;
using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    public class PlayCard
    {
        [ProtoMember(1)]
        public int CardValue { get; set; }
        [ProtoMember(2)]
        public int CardColor { get; set; }
    }
}
