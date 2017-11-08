using ProtoBuf;

namespace Coinche.Common.PacketType
{
    // TODO: Use enum
    [ProtoContract]
    public class PlayCard
    {
        [ProtoMember(1)]
        public int CardValue { get; set; }
        [ProtoMember(2)]
        public int CardColor { get; set; }
    }
}
