using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    public sealed class PlayCard
    {
        [ProtoMember(1)]
        public Core.Cards.CardType CardValue { get; set; }
        [ProtoMember(2)]
        public Core.Cards.CardColor CardColor { get; set; }
    }
}
