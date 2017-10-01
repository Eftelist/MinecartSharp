using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.Networking.Interfaces;
using MinecartSharp.Networking.Objects;
using MinecartSharp.Networking.Wrappers;
using Newtonsoft.Json;

namespace MinecartSharp.Networking.Packets
{
    class Disconnect : IPacket
    {
        public int PacketID
        {
            get
            {
                return 0x40;
            }
        }

        public bool IsPlayePacket
        {
            get
            {
                return true;
            }
        }

        public void Read(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {

        }

        public void Write(ClientWrapper state, MSGBuffer buffer, object[] Arguments)
        {
            if (state.PlayMode)
            {
                buffer.WriteVarInt(PacketID);
            }
            else
            {
                buffer.WriteVarInt(0x00);
            }
            buffer.WriteString(JsonConvert.SerializeObject((ChatMessage)Arguments[0], new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
            buffer.FlushData();
        }
    }
}
