using System;
using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    class Contract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public int Mode { get; set; } //TODO: we have enum for that
        [ProtoMember(3)]
        public int Color { get; set; }
        [ProtoMember(4)]
        public int Value { get; set; }
        [ProtoMember(5)]
        public bool IsCoinche { get; set; } = false;

    }
}
