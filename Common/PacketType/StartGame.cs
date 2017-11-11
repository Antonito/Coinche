using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    public sealed class StartGame
    {
        [ProtoMember(1)]
        public bool IsReady { get; set; }

    }
}
