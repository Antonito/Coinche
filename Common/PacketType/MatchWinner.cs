using ProtoBuf;

namespace Coinche.Common.PacketType
{
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
}
