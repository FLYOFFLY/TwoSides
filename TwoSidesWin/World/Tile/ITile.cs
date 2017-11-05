using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TwoSides.GameContent.GenerationResources;
using TwoSides.GameContent.Tiles;
using TwoSides.Physics.Entity;
using TwoSides.Utils;
using TwoSides.World.Generation;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
namespace TwoSides.World.Tile
{
    [Serializable]
    public sealed class Tile
    {
        public const int TileMaxSize = 16;
        public byte WaterType;
        public bool Active;
        public short Light;
        public short Blockheight;
        public float Hp { get; set; }
        public short IdTexture;
        public short IdWall;
        public short IdPoster;
        public byte IdSubTexture;
        static BaseTile[] _tileListCurrent;

        readonly BaseDimension _dimension;


        public enum SIde
        {
            LEFT = 0,
            CENTER = 1,
            RIGHT = 2,
            ONE = 3
        }

        /// <exception cref="OverflowException">The array is multIdimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
        public static void SetTileList(BaseTile[] tileList)
        {
            _tileListCurrent = tileList;
            _tileMax = tileList.Length;
        }

        static int _tileMax;
        public byte Frame;
        public ITileDatecs TileDate;
        public Tile(BaseDimension dimension)
        {
            _dimension = dimension;
            IdWall = -1;
            IdPoster = -1;
            Blockheight = 1;
            WaterType = 0;
            Frame = 0;
            IdSubTexture = 0;
        }
        public int GetSoildType() => IdTexture < _tileMax ? _tileListCurrent[IdTexture].GetSoildType() : 1;
        public bool IsSolid() => Active && _tileListCurrent[IdTexture].IsSolid();

        public void DamageSlot(float dmg,Vector2 position)
        {
            Hp -= dmg / Blockheight;
            _dimension.Dusts.Add(new Dust(position,_dimension.Rand));
        }

        public void UpdateHp() => Hp = GetBlockHp();

        public void UpdateTime()
        {
            TimeCount++;
            if ( GetAnimframe() < 2 ) return;

            if ( TimeCount < GetTickFrame() ) return;

            if (GetAnimframe() > Frame + 1) Frame++;
            else Frame = 0;
            TimeCount = 0;
        }
        public void Update(int x, int y, Camera camera)
        {
            UpdateTime();
            if (IdTexture <TileTwoSides.TileMax)
                _tileListCurrent[IdTexture].Update(x,y,_dimension,camera.InView(new Point(x * TileMaxSize, y * TileMaxSize)));
        }
        public float GetBlockHp() => _tileListCurrent[IdTexture].MaxHp;

        short GetLightBlock(int x, int y, bool wall)
        {
            if ( x <= 0 || x >= SizeGeneratior.WorldWidth || y <= 0 || y >= SizeGeneratior.WorldHeight ) return 0;

            if ( _dimension.MapTile[x , y].IsSolid() && _dimension.MapTile[x , y].HasShadow() )
                return (short) (_dimension.MapTile[x , y].Light - 10);

            if (!wall) return (short)(_dimension.MapTile[x, y].Light - 1);

            return (short)(_dimension.MapTile[x, y].Light - 3);
        }

        public void SetLight(int x, int y, short britghtess,int maxbritghtess)
        {
            if (y < _dimension.MapHeight[x] && y < maxbritghtess)
            {
                Light = britghtess;
            }
        }
        public void UpdateTileDate()
        {
            if ( IdTexture >= _tileMax ) return;

            TileDate = _tileListCurrent[IdTexture].ChangeTile();
            TileDate?.Init();
        }
        public Rectangle GetBoxRect(int x, int y) => IdTexture < _tileMax ? _tileListCurrent[IdTexture].GetBoxRect(x, y, this) : new Rectangle(0, 0, 16, 16);
        bool IsSolid(int x, int y) => _dimension.MapTile[x, y].IsSolid();

        public  bool IsLightBlock() => _tileListCurrent[IdTexture].IsLightBlock();

        public void UpdateLight(int x, int y)
        {
            if (IsLightBlock()) Light = 30;
            Light = Math.Max(Light, GetLightBlock(x + 1, y, IsSolid(x, y) && _dimension.MapTile[x,y].HasShadow()));
            Light = Math.Max(Light, GetLightBlock(x - 1, y, IsSolid(x, y) && _dimension.MapTile[x, y].HasShadow()));
            Light = Math.Max(Light, GetLightBlock(x, y + 1, IsSolid(x, y) && _dimension.MapTile[x, y].HasShadow()));
            Light = Math.Max(Light, GetLightBlock(x, y - 1, IsSolid(x, y) && _dimension.MapTile[x, y].HasShadow()));
            //  light = Math.Min(52, GetLightBlock(x + 1, y)+ GetLightBlock(x - 1, y)+ GetLightBlock(x, y+1)+ GetLightBlock(x, y-1));
        }


        public Tile[] GetSIdeBlock(int x, int y)
        {
            Tile[] tileSIde = new Tile[8];
            
            if(x> 0 && y> 0)tileSIde[0] = _dimension.MapTile[x-1,y-1];
            if (y > 0) tileSIde[1] = _dimension.MapTile[x, y - 1];
            if (x < SizeGeneratior.WorldWidth - 1 && y > 0) tileSIde[2] = _dimension.MapTile[x + 1, y - 1];
            if (x > 0) tileSIde[3] = _dimension.MapTile[x - 1, y];
            if (x < SizeGeneratior.WorldWidth - 1 && y > 0) tileSIde[4] = _dimension.MapTile[x + 1, y];
            if (x > 0 && y < SizeGeneratior.WorldHeight - 1) tileSIde[5] = _dimension.MapTile[x - 1, y + 1];
            if (y > 0) tileSIde[6] = _dimension.MapTile[x, y + 1];
            if (x < SizeGeneratior.WorldWidth - 1 && y < SizeGeneratior.WorldHeight - 1) tileSIde[7] = _dimension.MapTile[x + 1, y + 1];
            return tileSIde;
        }
        public bool GetActive(int x,int y)
        {
            if (x < 0 || x >= SizeGeneratior.WorldWidth) return false;
            if (y < 0 || y >= SizeGeneratior.WorldHeight) return false;
            return _dimension.MapTile[x,y].Active;
        }
        public SIde GetSIde(int i, int j)
        {
            if (IdTexture >= _tileMax) return SIde.CENTER;
            Tile[] tileSIde = GetSIdeBlock(i,j);
            if (GetActive(i+1,j) && _dimension.MapTile[i + 1, j].IdTexture == IdTexture && _dimension.MapHeight[i + 1] == j &&
                GetActive(i-1,j) && _dimension.MapTile[i - 1, j].IdTexture == IdTexture && _dimension.MapHeight[i - 1] == j) return SIde.CENTER;

            if (i-1>0 &&  tileSIde[3].IdTexture == IdTexture && tileSIde[3].Active && _dimension.MapHeight[i - 1] == j) return SIde.RIGHT;

            if (i+1<SizeGeneratior.WorldWidth && tileSIde[4].IdTexture == IdTexture && tileSIde[4].Active && _dimension.MapHeight[i + 1] == j) return SIde.LEFT;

            return SIde.ONE;
        }

        public int GetAnimframe() => _tileListCurrent[IdTexture].GetAnimFrame();

        public int GetIdSideTexture()
        {
            if (IdTexture < _tileMax)
            return _tileListCurrent[IdTexture].GetIdSideTexture();
            return -1;
        }

        public bool HasSpecialTexture() => GetIdSideTexture()>=0;

        public bool HasShadow() => _tileListCurrent[IdTexture].HasShadow();

        /// <exception cref="EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">The stream is closed. </exception>
        public void Read(BinaryReader reader,int version)
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
            Active = reader.ReadBoolean();
            if (version <= new NamingVersion(0, 0, 0, 0, 2, 0).GetCode())
            {
                reader.ReadBoolean();
                Light = (short)reader.ReadInt32();
                Blockheight = (short)reader.ReadInt32();
            }
            else
            {
                Light = reader.ReadInt16();
                Blockheight = reader.ReadInt16();
            }
            Hp = (float)reader.ReadDouble();
            IdTexture = reader.ReadInt16();
            IdSubTexture = reader.ReadByte();
            //TODO
            if (Active && IdTexture >= 0 && IdTexture < _tileMax)
            {
                TileDate = _tileListCurrent[IdTexture].ChangeTile();
            }
            IdWall = reader.ReadInt16();
            IdPoster = reader.ReadInt16();
        }

        /// <exception cref="IOException">An I/O error occurs. </exception>
        /// <exception cref="ObjectDisposedException">The stream is closed. </exception>
        public void Save(BinaryWriter writer)
        {
            writer.Write(Active);
            writer.Write(Light);
            writer.Write(Blockheight);
            writer.Write((double)Hp);
            writer.Write(IdTexture);
            writer.Write(IdSubTexture);
            //TODO
            writer.Write(IdWall);
            writer.Write(IdPoster);
        }
        public byte TimeCount;

        bool SwapWater(int i,int j,int i1,int j1){
            if (i1 < 0 || i1>=SizeGeneratior.WorldWidth) return false;
            if (j1 < 0 || j1 >= SizeGeneratior.WorldHeight) return false;
            if (_dimension.MapTile[i1, j1].IsSolid()) return false;
            if(_dimension.MapTile[i1, j1].WaterType > 0) return false;
            _dimension.MapTile[i1, j1].WaterType = 1;
            //dimension.map[i1, j1].patern = new Point(i,j);
            Console.WriteLine($"OLD x:{i} OLD Y:{j}");
            Console.WriteLine($"new x:{i1} new Y:{j1}");
            return true;
        }
        public void  Render(int i,int j,Vector2 pos, SpriteBatch spriteBatch,Texture2D water)
        {
            if (WaterType <= 0) return;
            if (IsSolid() )
            {
                WaterType = 0;
                return;
            }
            if (SwapWater(i, j, i, j + 1))
            {
                _dimension.MapTile[i, j + 1].Blockheight = 1;
            }
            if(Blockheight<=7) AddWater(i, j);
            spriteBatch.Draw(water, new Rectangle((int)pos.X, (int)pos.Y, 16,16), new Rectangle(0, 0, 16, 16), Color.Blue);
        }

        void AddWater(int i, int j)
        {
            if ( !_dimension.MapTile[i , j + 1].IsSolid() ) return;

            if (SwapWater(i, j, i + 1, j))
            {
                _dimension.MapTile[i + 1, j].Blockheight = (short)(Blockheight+1);
            }
            else if (SwapWater(i, j, i - 1, j))
            {
                _dimension.MapTile[i - 1, j].Blockheight = (short)(Blockheight+1);
            }
        }
        int GetTickFrame() => IdTexture < _tileMax ? _tileListCurrent[IdTexture].GetTickFrame() : 9999;

        public void Render(int i,int j, SpriteBatch spriteBatch, bool isGround, Texture2D[] textures, Texture2D[] addtexture, Vector2 pos, Color color)
        {
            UpdateTime();
            if (!HasSpecialTexture() || !isGround)
            {
                _tileListCurrent[IdTexture].Render(TileDate, spriteBatch, textures[IdTexture], _dimension, pos, i, j, Frame, IdSubTexture, color);

            }
            else if (HasSpecialTexture())
            {
                _tileListCurrent[IdTexture].Render(TileDate, spriteBatch, addtexture[GetIdSideTexture() + (int)GetSIde(i, j)], _dimension, pos, i, j, Frame, IdSubTexture, color);
            }
        }

        public void Update(int x, int y,DynamicEntity entity)
        {
            if (!Active) return;
            if (IdTexture < _tileMax)
                _tileListCurrent[IdTexture].Update(x, y,_dimension,entity);
        }
        public bool IsNeadTool(Item item) => _tileListCurrent[IdTexture].IsNeadTool(item);

        public bool Blockuse(int x,int y,DynamicEntity entity) => _tileListCurrent[IdTexture].UseBlock(x,y,_dimension,entity);

        public List<Item> Destory(int x,int y,DynamicEntity entity)
        {
            List<Item> dropList = new List<Item>();
            if (IdTexture < _tileMax)
            dropList =  _tileListCurrent[IdTexture].Destory(x,y,_dimension,entity);
            _dimension.Reset(x, y);

            if(_dimension.MapTile[x+1,y+1].Active)_dimension.MapTile[x + 1, y + 1].Update(x + 1, y + 1, entity);
            if(_dimension.MapTile[x+1,y].Active)_dimension.MapTile[x + 1, y].Update(x + 1, y, entity);
            if(_dimension.MapTile[x+1,y-1].Active)_dimension.MapTile[x + 1, y - 1].Update(x + 1, y - 1, entity);
            if(_dimension.MapTile[x-1,y+1].Active)_dimension.MapTile[x - 1, y + 1].Update(x - 1, y + 1, entity);
            if(_dimension.MapTile[x-1,y].Active)_dimension.MapTile[x - 1, y].Update(x - 1, y, entity);
            if(_dimension.MapTile[x-1,y-1].Active)_dimension.MapTile[x - 1, y - 1].Update(x - 1, y - 1, entity);
            if(_dimension.MapTile[x,y+1].Active)_dimension.MapTile[x, y + 1].Update(x, y + 1, entity);
            if(_dimension.MapTile[x,y-1].Active)_dimension.MapTile[x, y - 1].Update(x, y - 1, entity);

            return dropList;
        }


        public void InTile(DynamicEntity entity) => _tileListCurrent[IdTexture].InTile(entity);

        public bool AddedBlock(int id,int x,int y) => _tileListCurrent[id].BlockAdded(_dimension, x, y);
    }
}   
