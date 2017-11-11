using ProtoBuf;

namespace Coinche.Common.PacketType
{
    [ProtoContract]
    public class StartGame
    {
        [ProtoMember(1)]
        public bool IsReady { get; set; }

    }
}
