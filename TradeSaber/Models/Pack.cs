using System.Collections.Generic;

namespace TradeSaber.Models
{
    public class Pack
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; } = 5;
        public List<ProbabilityDatum> LockedCardPool { get; set; } = new List<ProbabilityDatum>();
        public List<string> GuaranteedCards { get; set; }
        public List<Rarity> GuaranteedRarities { get; set; } = new List<Rarity>();
        public string Theme { get; set; } = "#ffffff";
        public string CoverURL { get; set; }

        public class ProbabilityDatum
        {
            public string Id { get; set; }
            public double ProbabilityBoost { get; set; }
        }
    }
}