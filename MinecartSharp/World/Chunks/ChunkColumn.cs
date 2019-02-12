using System;
using System.IO;
using System.Net;
using MinecartSharp.Networking.Helpers;
using MinecartSharp.World.Blocks;
using MinecartSharp.World.NBT;

namespace MinecartSharp.World.Chunks
{
    public class ChunkColumn
    {
        public int X { get; set; }
        public int Z { get; set; }
        public byte[] BiomeId = ArrayOf<byte>.Create(256, 1);
        public int[] BiomeColor = ArrayOf<int>.Create(256, 1);

        public ushort[] Blocks = new ushort[16*16*256];
        public NibbleArray Skylight = new NibbleArray(16*16*256);
        public NibbleArray Blocklight = new NibbleArray(16*16*256);

        private byte[] _cache = null;

        public ChunkColumn()
        {
            for (int i = 0; i < Skylight.Length; i ++)
                Skylight [i] = 0xff;
            for (int i = 0; i < BiomeColor.Length; i++)
                BiomeColor[i] = 8761930; // Grass color?
        }

        public ushort GetBlock(int x, int y, int z)
        {
			return Blocks[x + 16 * z + 16 * 16 * y];
        }

		public byte GetMetadata(int x, int y, int z)
		{
			//return metadata[(x * 2048) + (z * 128) + y];
			return 0; //We dont support METADATA for now :P
		}

        public void SetBlock(int x, int y, int z, Block block)
        {
            int index = x + 16*z + 16*16*y;
            Blocks[index] = Convert.ToUInt16((block.Id << 4) | block.Metadata);
        }

        public void SetBlocklight(int x, int y, int z, byte data)
        {
            _cache = null;
            Blocklight[(x*2048) + (z*256) + y] = data;
        }

        public void SetSkylight(int x, int y, int z, byte data)
        {
            _cache = null;
            Skylight[(x*2048) + (z*256) + y] = data;
        }

        public byte[] GetBytes()
        {

            using (var stream = new MemoryStream())
            {
                using (var writer = new NbtBinaryWriter(stream, true))
                {
                    writer.Write(IPAddress.HostToNetworkOrder(X));
                    writer.Write(IPAddress.HostToNetworkOrder(Z));
                    writer.Write(true);
                    writer.Write((ushort) 0xffff); // bitmap
                    writer.WriteVarInt((Blocks.Length * 2) + Skylight.Data.Length + Blocklight.Data.Length +
                                       BiomeId.Length);

                    foreach (ushort i in Blocks)
                        writer.Write(i);

                    writer.Write(Blocklight.Data);
                    writer.Write(Skylight.Data);

                    writer.Write(BiomeId); //OK

                    writer.Flush();
                    writer.Close();
                }

                return stream.ToArray();
            }

        }
    }

    public static class ArrayOf<T> where T : new()
    {
        public static T[] Create(int size, T initialValue)
        {
            var array = new T[size];
            for (int i = 0; i < array.Length; i++)
                array[i] = initialValue;
            return array;
        }

        public static T[] Create(int size)
        {
            var array = new T[size];
            for (int i = 0; i < array.Length; i++)
                array[i] = new T();
            return array;
        }
    }

}