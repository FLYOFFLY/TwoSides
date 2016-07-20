using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using TwoSides.World;
namespace TwoSides
{
    sealed public class Recipe
    {
        public bool isblock { get; private set; }
        public int idblock { get; private set; }
        public float hp { get; private set; }       

        ArrayList igr = new ArrayList();
        ArrayList count = new ArrayList();

        public Item slot;                    
                                            
        public Recipe(Item slot,int id, float hp )
        {
            this.slot = slot;
            idblock = id;
            isblock = true;
            this.hp = hp;
        }
        
        public Recipe(Item slot, float hp)
        {
            this.slot = slot;
            isblock = false;
            this.hp = hp;
        }
        
        public int getsize()
        {
            return this.count.Count;
        }
        
        public int getigr(int id)
        {
            return (int)igr[id];
        }
        
        public int getconut(int id)
        {
            return (int)count[id];
        }
       
        public void addigr(int igr, int count)
        {
            this.igr.Add(igr);
            this.count.Add(count);
        }
    }
}
