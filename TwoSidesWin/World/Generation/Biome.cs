using System;
using System.IO;

using Microsoft.Xna.Framework;

using TwoSides.GameContent.GenerationResources;
using TwoSides.Utils;

namespace TwoSides.World.Generation
{
    [Serializable]
    public sealed class Biome
    {
        public ColorScheme Color{ get;  private set; }

        public int Temperature { get; private set; }
        public int Id { get; private set; }

        public int MaxHeight { get; set; }
        public int MinHeight { get; set; }

        public int StoneBlock { get; set; }
        public int TopBlock { get; set; }

        public Biome()
        {
            // TODO: Complete member initialization
        }
        public override bool Equals(object obj) => Equals(obj as Biome);

        public bool Equals(Biome obj) => Id == obj?.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public Biome(int temperature,int id,int minHeight, int maxHeight,int topBlock,int stoneBlock,Color color)
        {
            Temperature = temperature;
            Id = id;
            MaxHeight = maxHeight;
            MinHeight = minHeight;
            TopBlock = topBlock;
            StoneBlock = stoneBlock;
            Color = new ColorScheme(color);
        }
        public void Place(int x, BaseDimension dimension, int wIdth, int[] mapHeight)
        {
            for (var i = x; i < wIdth+x; i++)
            {
                for (var j = mapHeight[i]; j < SizeGeneratior.WorldHeight; j++)
                {
                    if ( dimension.MapTile[i , j].Active ) continue;

                    dimension.SetTexture(i , j , j < SizeGeneratior.RockLayer ? TopBlock : StoneBlock);
                }
                dimension.MapBiomes[i] = this;
            }
        }



        public void Read(BinaryReader reader)
        {
            /*
                public bool active = false;
                public bool infected = false;
                public int light = 0;
                public int blockheight = 1;
                public float HP { get; set; }
                public short Idtexture;
                public short IdWall;
                public short IdPoster; 
             */
            Color = Tools.ReadColor(reader);
            Temperature = reader.ReadInt32();
            Id = reader.ReadInt32();
            MinHeight = reader.ReadInt32();
            MaxHeight = reader.ReadInt32();
            TopBlock = reader.ReadInt32();
            StoneBlock = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            Tools.SaveColor(Color, writer);
            writer.Write(Temperature);
            writer.Write(Id);
            writer.Write(MinHeight);
            writer.Write(MaxHeight);
            writer.Write(TopBlock);
            writer.Write(StoneBlock);
        }
    }
}
