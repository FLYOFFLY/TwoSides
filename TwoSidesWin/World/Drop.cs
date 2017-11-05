namespace TwoSides.World
{
    public class Drop
    {
        public int MaxCount;
        public int MinCount;
        public int Сhance;
        public Item Item;
        public Drop(int maxCount,int minCount,int chance,Item item)
        {
            MinCount   = minCount;
            MaxCount   = maxCount;
            Сhance     = chance;
            Item = item;
        }
    }

}
