using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent;
using TwoSides.GameContent.Tiles;
using Microsoft.Xna.Framework.Graphics;
using TwoSides.World.Generation;
using TwoSides.Physics.Entity;
using System.IO;
using TwoSides.ModLoader;
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
namespace TwoSides.World.Tile
{
    [Serializable]
    sealed public class ITile
    {
        public const int TileMaxSize = 16;
        public byte waterType = 0;
        public bool active = false;
        public bool infected = false;
        public int light = 0;
        public int blockheight = 1;
        public float HP { get; set; }
        public short idtexture;
        public short wallid;
        public short posterid;
        public byte subTexture;
        static BaseTile[] tileListCurrent;
        
        BaseDimension dimension;


        public enum Side
        {
            left = 0,
            center = 1,
            right = 2,
            one = 3
        }

        public static Random rand = new Random();
        public static void setTileList(BaseTile[] tileList, int TileLimit)
        {
            tileListCurrent = tileList;
            TileMax = TileLimit;
        }
        public static int TileMax;
        public byte frame;
        public ITile(BaseDimension dimension)
        {
            this.dimension = dimension;
            wallid = -1;
            posterid = -1;
            blockheight = 1;
            waterType = 0;
            frame = 0;
            subTexture = 0;
        }
        public int getSoildType()
        {
            if (idtexture < TileMax)
                return tileListCurrent[idtexture].getSoildType();
            return 1;
        }
        public bool issolid()
        {
            bool s = false;

            if (idtexture >= TileMax) s = ((BlockMod)ModLoader.ModLoader.BlocksMods[ModLoader.ModLoader.getIndexBlocks(idtexture)]).issolid();
            else s = tileListCurrent[idtexture].issolid();
            return active && s;
        }

        public void damageslot(float dmg)
        {
            HP -= dmg / blockheight;
        }

        public void updateHP()
        {
            HP = getBlockHP();
        }
        public void updateTime()
        {
            timeCount++;
            if (getAnimframe() >= 2)
            {
                if (timeCount >= getTickFrame())
                {
                    if (getAnimframe() > frame + 1) frame++;
                    else frame = 0;
                    timeCount = 0;
                }
            }
        }
        public void Update(int x, int y, Camera camera)
        {
            updateTime();
            if (idtexture <TileTwoSides.TileMax)
                tileListCurrent[idtexture].Update(x,y,dimension,camera.inView(new Point(x * TileMaxSize, y * TileMaxSize)));
        }
        public float getBlockHP()
        {
            if (idtexture >= TileTwoSides.TileMax)
                return ((BlockMod)ModLoader.ModLoader.BlocksMods[ModLoader.ModLoader.getIndexBlocks(idtexture)]).HP;
            return tileListCurrent[idtexture].maxHP;
        }

        int getlb(int x, int y, bool wall)
        {
            if (x > 0 && x < SizeGeneratior.WorldWidth && y > 0 && y < SizeGeneratior.WorldHeight)
            {
                if (!dimension.map[x, y].issolid() || !dimension.map[x, y].hasShadow())
                {
                    if (!wall) return dimension.map[x, y].light - 1;
                    else return dimension.map[x, y].light - 3;
                }
                else return dimension.map[x, y].light - 10;
            }

            return 0;
        }

        public void lightu(int x, int y, int britghtess,int maxbritghtess)
        {
            if (y < dimension.mapHeight[x] && y < maxbritghtess)
            {
                light = britghtess;
            }
        }
        public Rectangle getBoxRect(int x, int y)
        {

            if (idtexture < TileMax)
                return tileListCurrent[idtexture].getBoxRect(x, y, this);
            else return new Rectangle(0, 0, 16, 16);
        }
        bool issolid(int x, int y)
        {
            return dimension.map[x, y].issolid();
        }
        public  bool isLightBlock()
        {
            if (idtexture >= TileMax) return ((BlockMod)ModLoader.ModLoader.BlocksMods[ModLoader.ModLoader.getIndexBlocks(idtexture)]).isLightBlock();
            else return tileListCurrent[idtexture].isLightBlock();
        }
        public void lightupdate(int x, int y)
        {
            if (isLightBlock()) light = 30;
            light = Math.Max(light, getlb(x + 1, y, issolid(x, y) && dimension.map[x,y].hasShadow()));
            light = Math.Max(light, getlb(x - 1, y, issolid(x, y) && dimension.map[x, y].hasShadow()));
            light = Math.Max(light, getlb(x, y + 1, issolid(x, y) && dimension.map[x, y].hasShadow()));
            light = Math.Max(light, getlb(x, y - 1, issolid(x, y) && dimension.map[x, y].hasShadow()));
            //  light = Math.Min(52, getlb(x + 1, y)+ getlb(x - 1, y)+ getlb(x, y+1)+ getlb(x, y-1));
        }


        public ITile[] getSideBlock(int x, int y)
        {
            ITile[] tileSide = new ITile[8];
            
            if(x> 0 && y> 0)tileSide[0] = dimension.map[x-1,y-1];
            if (y > 0) tileSide[1] = dimension.map[x, y - 1];
            if (x < SizeGeneratior.WorldWidth - 1 && y > 0) tileSide[2] = dimension.map[x + 1, y - 1];
            if (x > 0) tileSide[3] = dimension.map[x - 1, y];
            if (x < SizeGeneratior.WorldWidth - 1 && y > 0) tileSide[4] = dimension.map[x + 1, y];
            if (x > 0 && y < SizeGeneratior.WorldHeight - 1) tileSide[5] = dimension.map[x - 1, y + 1];
            if (y > 0) tileSide[6] = dimension.map[x, y + 1];
            if (x < SizeGeneratior.WorldWidth - 1 && y < SizeGeneratior.WorldHeight - 1) tileSide[7] = dimension.map[x + 1, y + 1];
            return tileSide;
        }
        public Side getside(int i, int j)
        {
            if (idtexture >= TileMax) return Side.center;
            ITile[] tileSide = getSideBlock(i,j);
            if (i - 1 > 0 && i + 1 > SizeGeneratior.WorldWidth - 1)
            {
                if (tileSide[3].idtexture == idtexture && tileSide[3].active && dimension.mapHeight[i - 1] == j) return Side.right;
                else return Side.one;
            }
            else if (i - 1 < 0 && i + 1 < SizeGeneratior.WorldWidth - 1)
            {
                if (tileSide[4].idtexture == idtexture && tileSide[4].active && dimension.mapHeight[i + 1] == j) return Side.left;
                else return Side.one;
            }
            else
            {
                if (dimension.map[i + 1, j].idtexture == idtexture && dimension.map[i + 1, j].active && dimension.mapHeight[i + 1] == j &&
                    dimension.map[i - 1, j].idtexture == idtexture && dimension.map[i - 1, j].active && dimension.mapHeight[i - 1] == j) return Side.center;
                else if (dimension.map[i + 1, j].idtexture == idtexture && dimension.map[i + 1, j].active && dimension.mapHeight[i + 1] == j || (
                    dimension.map[i - 1, j].idtexture == idtexture && dimension.map[i - 1, j].active && dimension.mapHeight[i - 1] == j))
                {
                    if (dimension.map[i + 1, j].idtexture == idtexture && dimension.map[i + 1, j].active && dimension.mapHeight[i + 1] == j) return Side.left;
                    else return Side.right;
                }
                else return Side.one;
            }
        }

        public int getAnimframe()
        {
            if (idtexture < TileMax)
                return tileListCurrent[idtexture].getAnimFrame();
            else return 1;
        }

        public int getIDSideTexture()
        {
            if (idtexture < TileMax)
            return tileListCurrent[idtexture].getIDSideTexture();
            return -1;
        }

        public bool hasSpecialTexture()
        {
            return getIDSideTexture()>=0;
        }
        public bool hasShadow()
        {
            if (idtexture < TileMax)
                return tileListCurrent[idtexture].hasShadow();
            else
                return true;
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
            active = reader.ReadBoolean();
            infected = reader.ReadBoolean();
            light = reader.ReadInt32();
            blockheight = reader.ReadInt32();
            HP = (float)reader.ReadDouble();
            idtexture = reader.ReadInt16();
            wallid = reader.ReadInt16();
            posterid = reader.ReadInt16();
        }

        public void save(BinaryWriter writer)
        {
            writer.Write(active);
            writer.Write(infected);
            writer.Write(light);
            writer.Write(blockheight);
            writer.Write((Double)HP);
            writer.Write(idtexture);
            writer.Write(wallid);
            writer.Write(posterid);
        }
        public byte timeCount = 0;
        private bool swapWater(int i,int j,int i1,int j1){
            if (i1 < 0 || i1>=SizeGeneratior.WorldWidth) return false;
            if (j1 < 0 || j1 >= SizeGeneratior.WorldHeight) return false;
            if (dimension.map[i1, j1].issolid()) return false;
            if(dimension.map[i1, j1].waterType > 0) return false;
            dimension.map[i1, j1].waterType = 1;
            //dimension.map[i1, j1].patern = new Point(i,j);
            System.Console.WriteLine("OLD x:" + i + " OLD Y:" + j);
            System.Console.WriteLine("new x:" + i1 + " new Y:" + j1);
            return true;
        }
        public void  Render(int i,int j,Vector2 pos, SpriteBatch spriteBatch,Texture2D water)
        {
            if (this.waterType <= 0) return;
            if (issolid() )
            {
                waterType = 0;
                return;
            }
            if (swapWater(i, j, i, j + 1))
            {
                dimension.map[i, j + 1].blockheight = 1;
            }
            if(this.blockheight<=7) addWater(i, j);
            spriteBatch.Draw(water, new Rectangle((int)pos.X, (int)pos.Y, 16,16), new Rectangle(0, 0, 16, 16), Color.Blue);
        }

        private void addWater(int i, int j)
        {
            if (dimension.map[i, j+1].issolid())
            {
                if (swapWater(i, j, i + 1, j))
                {
                    dimension.map[i + 1, j].blockheight = blockheight+1;
                }
                else if (swapWater(i, j, i - 1, j))
                {
                    dimension.map[i - 1, j].blockheight = blockheight+1;
                }
            }
        }
        int getTickFrame()
        {
            if (idtexture < TileMax)
                return tileListCurrent[idtexture].getTickFrame();
            else return 9999;
        }
        public void Render(int i,int j, SpriteBatch spriteBatch, bool isGround, Texture2D[] textures, Texture2D[] addtexture, Vector2 pos, Color color)
        {
            updateTime();
            if (!hasSpecialTexture() || !isGround)
            {
                if (idtexture < TileMax)
                    spriteBatch.Draw(textures[idtexture], new Rectangle((int)pos.X, (int)pos.Y, 16, 16), new Rectangle(16 * (frame + tileListCurrent[idtexture].getFrame(i,j,dimension,frame)), 16 * subTexture, 16, 16), color);
                else
                {
                    int idBlocks = ModLoader.ModLoader.getIndexBlocks(idtexture);
                    spriteBatch.Draw(((BlockMod)ModLoader.ModLoader.BlocksMods[idBlocks]).texture, pos, color);
                }
            }
            else if (hasSpecialTexture())
            {
                spriteBatch.Draw(addtexture[getIDSideTexture() + (int)getside(i, j)], new Rectangle((int)pos.X, (int)pos.Y, 16, 16), new Rectangle(16*frame, 0, 16, 16), color);
            }
        }

        public void update(int x, int y,CEntity entity)
        {
            if (!active) return;
            if (idtexture < TileMax)
                tileListCurrent[idtexture].update(x, y,dimension,entity);
        }
        public bool isNeadTool(Item item)
        {
            if (idtexture < TileMax)
                return tileListCurrent[idtexture].isNeadTool(item);
            else return true;
        }

        public bool blockuse(int x,int y,CEntity entity)
        {
            if (idtexture < TileMax)
                return tileListCurrent[idtexture].blockuse(x,y,dimension,entity);
            else return false;
        }

        public List<Item> destory(int x,int y,CEntity entity)
        {
            List<Item> dropList;
            if (idtexture < TileMax)
            dropList =  tileListCurrent[idtexture].destory(x,y,dimension,entity);
            else {
                int id = ModLoader.ModLoader.getIndexBlocks(idtexture);
                dropList = ((BlockMod)ModLoader.ModLoader.BlocksMods[id]).destory(id, entity);
            }
            dimension.Reset(x, y);

            if(dimension.map[x+1,y+1].active)dimension.map[x + 1, y + 1].update(x + 1, y + 1, entity);
            if(dimension.map[x+1,y].active)dimension.map[x + 1, y].update(x + 1, y, entity);
            if(dimension.map[x+1,y-1].active)dimension.map[x + 1, y - 1].update(x + 1, y - 1, entity);
            if(dimension.map[x-1,y+1].active)dimension.map[x - 1, y + 1].update(x - 1, y + 1, entity);
            if(dimension.map[x-1,y].active)dimension.map[x - 1, y].update(x - 1, y, entity);
            if(dimension.map[x-1,y-1].active)dimension.map[x - 1, y - 1].update(x - 1, y - 1, entity);
            if(dimension.map[x,y+1].active)dimension.map[x, y + 1].update(x, y + 1, entity);
            if(dimension.map[x,y-1].active)dimension.map[x, y - 1].update(x, y - 1, entity);

            return dropList;
        }

        public void send(Lidgren.Network.NetOutgoingMessage sendMsg)
        {
            sendMsg.Write(active);
            sendMsg.Write(infected);
            sendMsg.Write(light);
            sendMsg.Write(blockheight);
            sendMsg.Write((Double)HP);
            sendMsg.Write(idtexture);
            sendMsg.Write(wallid);
            sendMsg.Write(posterid);
        }

        internal void read(Lidgren.Network.NetIncomingMessage msg)
        {
            throw new NotImplementedException();
        }

        public void InTile(CEntity entity)
        {
            tileListCurrent[idtexture].InTile(entity);
        }
    }
}   
