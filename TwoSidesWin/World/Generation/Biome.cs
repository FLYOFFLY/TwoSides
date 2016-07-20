using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoSides.GameContent.GenerationResources;
using System.IO;
using TwoSides.Utils;
using Lidgren.Network;

namespace TwoSides.World.Generation
{
    [Serializable]
    sealed public class Biome
    {
        public Color color{ get;  private set; }

        public int Temperature { get; private set; }
        public int id { get; private set; }

        public int maxHeight { get; set; }
        public int minHeight { get; set; }

        public int stoneBlock { get; set; }
        public int TopBlock { get; set; }

        public Biome()
        {
            // TODO: Complete member initialization
        }
        public override bool Equals(Object obj)
        {
            return Equals(obj as Biome);
        }
        public bool Equals(Biome obj)
        {
            if (obj == null)
                return false;
            return id == obj.id;
        }
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
        public Biome(int t,int id,int minHeight, int MaxHeight,int TopBlock,int stoneBlock,Color color)
        {
            Temperature = t;
            this.id = id;
            this.maxHeight = MaxHeight;
            this.minHeight = minHeight;
            this.TopBlock = TopBlock;
            this.stoneBlock = stoneBlock;
            this.color = color;
        }
        public void Place(int x, BaseDimension dimension, int width, int[] mapHeight)
        {
            for (int i = x; i < width+x; i++)
            {
                for (int j = mapHeight[i]; j < SizeGeneratior.WorldHeight; j++)
                {
                    if (!dimension.map[i, j].active)
                    {
                        if (j < SizeGeneratior.rockLayer) dimension.settexture(i, j, this.TopBlock);
                        else dimension.settexture(i, j, this.stoneBlock);
                    }
                }
                dimension.mapB[i] = this;
            }
        }



        public void read(BinaryReader reader)
        {
            /*
                public bool active = false;
                public bool infected = false;
                public int light = 0;
                public int blockheight = 1;
                public float HP { get; set; }
                public short idtexture;
                public short wallid;
                public short posterid; 
             */
            color = Util.readColor(reader);
            Temperature = reader.ReadInt32();
            id = reader.ReadInt32();
            minHeight = reader.ReadInt32();
            maxHeight = reader.ReadInt32();
            TopBlock = reader.ReadInt32();
            stoneBlock = reader.ReadInt32();
        }

        public void save(BinaryWriter writer)
        {
            Util.SaveColor(color, writer);
            writer.Write(Temperature);
            writer.Write(id);
            writer.Write(minHeight);
            writer.Write(maxHeight);
            writer.Write(TopBlock);
            writer.Write(stoneBlock);
        }

        public void send(NetOutgoingMessage sendMsg)
        {

            sendMsg.Write(Temperature);
            sendMsg.Write(id);
            sendMsg.Write(minHeight);
            sendMsg.Write(maxHeight);
            sendMsg.Write(TopBlock);
            sendMsg.Write(stoneBlock);
        }

        public void read(NetIncomingMessage msg)
        {
            Temperature = msg.ReadInt32();
            id = msg.ReadInt32();
            minHeight = msg.ReadInt32();
            maxHeight = msg.ReadInt32();
            TopBlock = msg.ReadInt32();
            stoneBlock = msg.ReadInt32();
        }
    }
}
