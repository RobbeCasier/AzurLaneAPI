using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALListViewer.Model
{
    class Ship
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "class")]
        public string ShipClass { get; set; }
        public string Nationality { get; set; }
        public string HullType { get; set; }
        public string Thumbnail { get; set; }
        public string Rarity { get; set; }
        [JsonProperty(PropertyName = "stars.stars")]
        public string Stars { get; set; }
        public Stats Stats { get; set; }
        public Slot[] Slots { get; set; }
        public Skill[] Skills { get; set; }
        public string[][] LimitBreaks { get; set; }
        [JsonProperty(PropertyName = "construction.constructiontime")]
        public string ConstructionTime { get; set; }
        public Skin[] Skins { get; set; }
        public bool Retrofit { get; set; }
        public string RetrofitId { get; set; }
        public string RetrofitHullType { get; set; }

        public uint Count { get; set; } = 0;
    }

    public class Basestats
    {
        public string Health { get; set; }
        public string Armor { get; set; }
        public string Reload { get; set; }
        public string Luck { get; set; }
        public string Firepower { get; set; }
        public string Torpedo { get; set; }
        public string Evasion { get; set; }
        public string Speed { get; set; }
        public string Antiair { get; set; }
        public string Aviation { get; set; }
        public string OilConsumption { get; set; }
        public string Accuracy { get; set; }
        public string AntisubmarineWarfare { get; set; }
        public string Oxygen { get; set; }
        public string Ammunition { get; set; }
        public string HuntingRange { get; set; }
    }

    public class Stats
    {
        public Basestats BaseStats { get; set; }
        public Basestats Level100 { get; set; }
        public Basestats Level120 { get; set; }
        public Basestats Level100Retrofit { get; set; }
        public Basestats Level120Retrofit { get; set; }

        public Basestats GetStats(string stats)
        {
            if (stats.Equals("BaseStats"))
                return BaseStats;
            else if (stats.Equals("Level100"))
                return Level100;
            else if (stats.Equals("Level120"))
                return Level120;
            else if (stats.Equals("Level100Retrofit"))
                return Level100Retrofit;
            else if (stats.Equals("Level120Retrofit"))
                return Level120Retrofit;
            return null;
        }
    }


    public class Slot
    {
        public int MaxEfficiency { get; set; }
        public int MinEfficiency { get; set; }
        public string Type { get; set; }
        public int KaiEfficiency { get; set; }
    }

    public class Skill
    {
        public string Icon { get; set; }
        [JsonProperty(PropertyName = "skills.names.en")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }

    public class Skin
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Background { get; set; }
        public string Bg { get; set; }
        public List<VoiceLine> Lines { get; set; }
    }

    public class VoiceLine
    {
        public string Event { get; set; }
        public string Audio { get; set; }
        public string En { get; set; }
    }

}
