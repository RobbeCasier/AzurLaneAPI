using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurLaneAPI.Model
{
    struct ShipDataList
    {
        public MyShip MyShip { get; set; }
        public Ship Ship { get; set; }
    }

    class MyShip
    {
        public string Id { get; set; }
        public ushort Lv { get; set; } = 1;
        public string HullType { get; set; }
        public string Rarity { get; set; }
        public Basestats CurrentStat { get; set; }
        public bool IsMarried { get; set; } = false;
        public bool IsRetrofitted { get; set; } = false;
        public ushort[] MySkins { get; set; } = new ushort[]{0};
        public ushort CurrentSkin { get; set; } = 0;
        public ushort CurrentAffection { get; set; } = 50;
    }
}
