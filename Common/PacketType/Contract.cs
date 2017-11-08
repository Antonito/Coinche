using System;
using ProtoBuf;

namespace Coinche.Common.PacketType
{
    // TODO: Use enum
    [ProtoContract]
    public class ContractResponse
    {
        [ProtoMember(1)]
        public int Promise { get; set; }
        [ProtoMember(2)]
        public int Color { get; set; }
    }

    [ProtoContract]
    public class ContractRequest
    {
        [ProtoMember(1)]
        public int MinimumValue { get; set; }
    }

    [ProtoContract]
    public class ContractInfo
    {
        [ProtoMember(1)]
        public int Promise { get; set; }
        [ProtoMember(2)]
        public int Color { get; set; }
        [ProtoMember(3)]
        public string Pseudo { get; set; }
    }
}
