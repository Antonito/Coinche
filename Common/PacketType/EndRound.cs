using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    public class EndRound
    {
        [ProtoMember(1)]
        public int WinnerTeam { get; set; }
        [ProtoMember(2)]
        public int WinnerPoint { get; set; }
        [ProtoMember(3)]
        public int LoserPoint { get; set; }
    }
}
