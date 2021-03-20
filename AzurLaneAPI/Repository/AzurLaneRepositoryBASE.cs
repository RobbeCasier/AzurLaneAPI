using AzurLaneAPI.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurLaneAPI.Repository
{
    public abstract class AzurLaneRepositoryBASE
    {
        protected List<Ship> _fullListShips;
        protected List<Ship> _filterList;
        protected List<Ship> _filterListByStat;

        public abstract Task<List<Ship>> GetShips();

        public List<Ship> GetCommonShips()
        {
            List<Ship> fullList = _fullListShips;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                int id;
                if (int.TryParse(ship.Id, out id))
                    if (id < 1000)
                        sortedList.Add(ship);
            }
            _filterListByStat = _filterList = sortedList;
            return _filterList;
        }

        public List<Ship> GetCollabsShips()
        {
            List<Ship> fullList = _fullListShips;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                int id;
                if (ship.Id.Contains("Collab"))
                    sortedList.Add(ship);
                else if (int.TryParse(ship.Id, out id))
                    if (id > 10000 && id < 30000)
                        sortedList.Add(ship);
            }
            _filterListByStat = _filterList = sortedList;
            return _filterList;
        }

        public List<Ship> GetPRShips()
        {
            List<Ship> fullList = _fullListShips;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                if (ship.Id.Contains("Plan"))
                    sortedList.Add(ship);
            }
            _filterListByStat = _filterList = sortedList;
            return _filterList;
        }

        public List<Ship> GetMETAShips()
        {
            List<Ship> fullList = _fullListShips;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                int id;
                if (int.TryParse(ship.Id, out id))
                    if (id > 30000)
                        sortedList.Add(ship);
            }
            _filterListByStat = _filterList = sortedList;
            return _filterList;
        }

        public List<Ship> SortByRarity()
        {
            _filterListByStat = _filterList;
            List<Ship> sortedList = new List<Ship>();
            List<Ship> CommonList = new List<Ship>();
            List<Ship> RareList = new List<Ship>();
            List<Ship> EliteList = new List<Ship>();
            List<Ship> SuperRareList = new List<Ship>();
            List<Ship> UltraRareList = new List<Ship>();

            foreach (Ship ship in _filterListByStat)
            {
                switch (ship.Rarity)
                {
                    case "Common":
                        CommonList.Add(ship);
                        break;
                    case "Rare":
                        RareList.Add(ship);
                        break;
                    case "Elite":
                        EliteList.Add(ship);
                        break;
                    case "Super Rare":
                    case "Priority":
                        SuperRareList.Add(ship);
                        break;
                    case "Ultra Rare":
                    case "Decisive":
                        UltraRareList.Add(ship);
                        break;
                    default:
                        break;
                }
            }
            sortedList.AddRange(UltraRareList);
            sortedList.AddRange(SuperRareList);
            sortedList.AddRange(EliteList);
            sortedList.AddRange(RareList);
            sortedList.AddRange(CommonList);
            _filterListByStat = sortedList;
            return _filterList;
        }

        public List<Ship> SortByIndex()
        {
            if (_filterList != null)
            {
                _filterListByStat = _filterList.OrderBy(x => x.Id).ToList();
                return _filterListByStat;
            }

            return null;
        }

        public List<Ship> SortByStat(string stat)
        {
            _filterListByStat = _filterList;
            List<KeyValuePair<int, Ship>> sortedList = new List<KeyValuePair<int, Ship>>();
            List<Ship> sort = new List<Ship>();
            foreach (Ship ship in _filterListByStat)
            {
                object value1Obj;
                Basestats level120Rets1 = ship.Stats.Level120Retrofit;
                Basestats level120s1 = ship.Stats.Level120;
                if (level120Rets1 == null)
                    value1Obj = level120s1.GetType().GetProperty(stat).GetValue(level120s1);
                else
                    value1Obj = level120Rets1.GetType().GetProperty(stat).GetValue(level120Rets1);
                int.TryParse(value1Obj.ToString(), out int value1Int);
                sortedList.Add(new KeyValuePair<int, Ship>(value1Int, ship));
            }
            sortedList = sortedList.OrderBy(x => x.Key).ToList();
            sortedList.Reverse();
            foreach (KeyValuePair<int, Ship> ship in sortedList)
            {
                sort.Add(ship.Value);
            }
            _filterListByStat = sort;
            return _filterListByStat;
        }

        public List<Ship> SortByIndication(List<string> indexes)
        {
            List<Ship> sortedList = new List<Ship>();
            foreach (Ship ship in _filterListByStat)
            {
                foreach (string type in indexes)
                {
                    if (ship.HullType.Equals(type))
                    {
                        sortedList.Add(ship);
                        break;
                    }
                }
            }
            _filterListByStat = sortedList;
            return _filterListByStat;
        }

        public List<Ship> SortByFaction(List<string> factions)
        {
            List<Ship> sortedList = new List<Ship>();
            foreach (Ship ship in _filterListByStat)
            {
                foreach (string faction in factions)
                {
                    if (ship.Nationality.Contains(faction))
                        sortedList.Add(ship);
                    else if (faction.Equals("FactionOther"))
                    {
                        if (!ship.Nationality.Contains("Eagle") &&
                            !ship.Nationality.Contains("Royal") &&
                            !ship.Nationality.Contains("Sakura") &&
                            !ship.Nationality.Contains("Iron") &&
                            !ship.Nationality.Contains("Dragon") &&
                            !ship.Nationality.Contains("Sardegna") &&
                            !ship.Nationality.Contains("Iris") &&
                            !ship.Nationality.Contains("Vichya") &&
                            !ship.Nationality.Contains("Northern"))
                            sortedList.Add(ship);
                    }
                }
            }
            _filterListByStat = sortedList;
            return _filterListByStat;
        }

        public List<Ship> SortByRarity(List<string> rarities)
        {
            List<Ship> sortedList = new List<Ship>();
            foreach (Ship ship in _filterListByStat)
            {
                foreach (string rarity in rarities)
                {
                    if (ship.Rarity.Contains(rarity))
                        sortedList.Add(ship);
                    else if (rarity.Equals("Super") && ship.Rarity.Equals("Priority"))
                        sortedList.Add(ship);
                    else if (rarity.Equals("Ultra") && ship.Rarity.Equals("Decisive"))
                        sortedList.Add(ship);
                }    
            }
            _filterListByStat = sortedList;
            return _filterListByStat;
        }

        public List<Ship> SearchShip(string name)
        {
            List<Ship> sortedList = new List<Ship>();
            string lowerCaseShip, lowerCaseName = name.ToLower();
            foreach (Ship ship in _filterListByStat)
            {
                lowerCaseShip = ship.Name.ToLower();
                if (lowerCaseShip.Contains(lowerCaseName))
                    sortedList.Add(ship);
            }
            return sortedList;
        }
    }
}
