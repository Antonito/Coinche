using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    public sealed class ContractResponse
    {
        [ProtoMember(1)]
        public Core.Contract.Promise Promise { get; set; }
        [ProtoMember(2)]
        public Core.Cards.CardColor Color { get; set; }
        [ProtoMember(3)]
        public Core.Game.GameMode GameMode { get; set; }
    }

    [ProtoContract]
    public sealed class ContractRequest
    {
        [ProtoMember(1)]
        public Core.Contract.Promise MinimumValue { get; set; }
    }

    [ProtoContract]
    public sealed class ContractInfo
    {
        [ProtoMember(1)]
        public Core.Contract.Promise Promise { get; set; }
        [ProtoMember(2)]
        public Core.Cards.CardColor Color { get; set; }
        [ProtoMember(3)]
        public string Pseudo { get; set; }
    }
}
