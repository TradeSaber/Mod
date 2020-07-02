namespace TradeSaber.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string Series { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Master { get; set; }
        public int MaxPrints { get; set; } = -1;
        public Rarity Rarity { get; set; } = Rarity.Common;
        public bool Locked { get; set; } = false;
        public double BaseProbability { get; set; } = .05f;
        public string CoverURL { get; set; }
        public string BaseURL { get; set; }
    }
}