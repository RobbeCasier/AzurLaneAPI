using AzurLaneAPI.Model;
using AzurLaneAPI.Repository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AzurLaneAPI.ViewModel
{
    class MyListVM : ObservableObject
    {
        public MainViewModel MainVM { get; set; }
        public AzurLaneRepositoryBASE _alRepository = new AzurLaneRepositoryBASE();

        //sorting variables
        private string SortingParameter { get; set; } = "cLv";
        private List<string> SortingParametersIndex { get; set; } = new List<string>() { "All" };
        private List<string> SortingParametersFaction { get; set; } = new List<string>() { "All" };
        private List<string> SortingParametersRarity { get; set; } = new List<string>() { "All" };

        public List<MyShip> MyList { get; set; } = new List<MyShip>();

        public List<ShipDataList> ShipDataLists { get; set; } = new List<ShipDataList>();
        public List<ShipDataList> FilterListStat { get; set; } = new List<ShipDataList>();

        public ShipDataList _selectedShip;
        public ShipDataList SelectedShip
        {
            get { return _selectedShip; }
            set 
            { 
                _selectedShip = value;
                if (value.Ship != null && value.MyShip != null)
                    MainVM.DetailMyListSwitch();
            }
        }

        public MyListVM()
        {
            GetMyList();
        }


        public void AddShip(Ship ship)
        {
            ushort[] skills1 = new ushort[ship.Skills.Length];
            for (ushort i = 0; i < skills1.Length; i++)
            {
                skills1[i] = 1;
            }
            MyShip newShip = new MyShip
            {
                Id = ship.Id,
                HullType = ship.HullType,
                Rarity = ship.Rarity,
                CurrentStat = ship.Stats.BaseStats,
                SKillLvs = skills1
            };
            MyList.Add(newShip);
            ShipDataLists.Add(new ShipDataList { MyShip = newShip, Ship = ship });
            ShipDataLists = ShipDataLists.OrderByDescending(x => x.MyShip.Lv).ToList();
            FilterListStat = ShipDataLists;
            RaisePropertyChanged("FilterListStat");
            WriteJson();
        }

        public void UpdateList()
        {
            ShipDataLists = ShipDataLists.OrderByDescending(x => x.MyShip.Lv).ToList();
            FilterListStat = ShipDataLists;
            RaisePropertyChanged("FilterListStat");
            WriteJson();
        }

        public RelayCommand ToDetailCommand
        {
            get
            {
                return new RelayCommand(ToDetail);
            }
        }

        private void ToDetail()
        {
            MainVM.DetailMyListSwitch();
        }

        private void WriteJson()
        {
            string json = JsonConvert.SerializeObject(MyList.ToArray());
            File.WriteAllText(@"D:\AzurLaneAPIList.json", json);
        }
        
        private void GetMyList()
        {
                var taskRes = Task.Run(() =>
                {
                    return _alRepository.GetShips();
                });
                taskRes.ConfigureAwait(true).GetAwaiter().OnCompleted(
                    () =>
                    {
                        if (File.Exists(@"D:\AzurLaneAPIList.json"))
                        {
                            string json = File.ReadAllText(@"D:\AzurLaneAPIList.json");
                            if (!json.Equals(""))
                            {
                                MyList = JsonConvert.DeserializeObject<List<MyShip>>(json);
                                MyList = MyList.OrderByDescending(x => x.Lv).ToList();
                                foreach (MyShip myShip in MyList)
                                {
                                    Ship tShip = taskRes.Result.Find(x => x.Id == myShip.Id);
                                    ShipDataLists.Add(new ShipDataList { MyShip = myShip, Ship = tShip });
                                }
                                FilterListStat = ShipDataLists;
                                RaisePropertyChanged("ShipDataLists");
                                RaisePropertyChanged("FilterListStat");
                            }
                        }
                    });
        }


        //FILTER COMMANDS
        public RelayCommand<object> SearchParametersCommand
        {
            get
            {
                return new RelayCommand<object>(SearchParameters);
            }
        }

        public RelayCommand<object> SearchParametersIndexCommand
        {
            get
            {
                return new RelayCommand<object>(SearchIndex);
            }
        }

        public RelayCommand<object> SearchParametersFaction
        {
            get
            {
                return new RelayCommand<object>(SearchFaction);
            }
        }

        public RelayCommand<object> SearchParametersRarity
        {
            get
            {
                return new RelayCommand<object>(SearchRarity);
            }
        }

        public RelayCommand FilterCommand
        {
            get
            {
                return new RelayCommand(Filter);
            }
        }

        public RelayCommand<object> SearchCommand
        {
            get
            {
                return new RelayCommand<object>(Search);
            }
        }

        //-------------//
        //FILTER SEARCH//
        //-------------//
        private void SearchParameters(object sender)
        {
            CheckBox checkBox = (CheckBox)sender;
            if ((bool)checkBox.IsChecked)
                SortingParameter = checkBox.Name;
            else
                SortingParameter = "cLv";
            RaisePropertyChanged("SortingParameter");
        }

        private void SearchIndex(object sender)
        {
            CheckBox checkBox = (CheckBox)sender;
            string name = checkBox.Name.Substring(1);
            if ((bool)checkBox.IsChecked)
            {
                SortingParametersIndex.Remove("All");
                if (name.Equals("Main") || name.Equals("Vanguard"))
                {
                    SortingParametersIndex = new List<string>() { name };
                }
                else if (name.Equals("IndexAll"))
                    SortingParametersIndex = new List<string>() { "All" };
                else
                {
                    if (SortingParametersIndex.Contains("Main") || SortingParametersIndex.Contains("Vanguard"))
                        SortingParametersIndex = new List<string>();
                    SortingParametersIndex.Add(name);
                }

            }
            else
            {
                if (SortingParametersIndex.Count() == 1)
                    SortingParametersIndex = new List<string>() { "All" };
                else
                    SortingParametersIndex.Remove(name);
            }
        }

        private void SearchFaction(object sender)
        {
            CheckBox checkBox = (CheckBox)sender;
            string name = checkBox.Name.Substring(1);
            if ((bool)checkBox.IsChecked)
            {
                if (name.Equals("AllFaction"))
                    SortingParametersFaction = new List<string>() { "All" };
                else
                {
                    SortingParametersFaction.Remove("All");
                    SortingParametersFaction.Add(name);
                }
            }
            else
            {
                if (SortingParametersFaction.Count() == 1)
                    SortingParametersFaction = new List<string>() { "All" };
                else
                    SortingParametersFaction.Remove(name);
            }

        }

        private void SearchRarity(object sender)
        {
            CheckBox checkBox = (CheckBox)sender;
            string name = checkBox.Name.Substring(1);
            if ((bool)checkBox.IsChecked)
            {
                if (name.Equals("RarityAll"))
                    SortingParametersRarity = new List<string>() { "All" };
                else
                {
                    SortingParametersRarity.Remove("All");
                    SortingParametersRarity.Add(name);
                }
            }
            else
            {
                if (SortingParametersRarity.Count() == 1)
                    SortingParametersRarity = new List<string>() { "All" };
                else
                    SortingParametersRarity.Remove(name);
            }
        }

        //------//
        //FILTER//
        //------//
        private void Filter()
        {
            if (SortingParameter != null)
            {
                if (SortingParameter.Equals("cLv"))
                    SortByLevel();
                else if (SortingParameter.Equals("cRarity"))
                    SortByRarity();
                else
                {
                    string stat = SortingParameter.Substring(1);
                    SortByStat(stat);
                }

            }
            if (!SortingParametersIndex[0].Equals("All"))
            {
                SortByIndication(GetterShipTypes());
            }
            if (!SortingParametersFaction[0].Equals("All"))
            {
                SortByFaction(SortingParametersFaction);
            }
            if (!SortingParametersRarity[0].Equals("All"))
            {
                SortByRarity(SortingParametersRarity);
            }
            RaisePropertyChanged("FilterListStat");

        }

        private List<string> GetterShipTypes()
        {
            List<string> types = new List<string>();
            foreach (string type in SortingParametersIndex)
            {
                if (type.Equals("DD"))
                    types.Add("Destroyer");
                else if (type.Equals("CL"))
                    types.Add("Light Cruiser");
                else if (type.Equals("CA"))
                {
                    types.Add("Heavy Cruiser");
                    types.Add("Large Cruiser");
                }
                else if (type.Equals("BB"))
                {
                    types.Add("Battleship");
                    types.Add("Battlecruiser");
                }
                else if (type.Equals("CV"))
                {
                    types.Add("Aircraft Carrier");
                    types.Add("Light Carrier");
                }
                else if (type.Equals("Repair"))
                    types.Add("Repair");
                else if (type.Equals("SS"))
                    types.Add("Submarine");
                else if (type.Equals("Vanguard"))
                {
                    types.Add("Destroyer");
                    types.Add("Light Cruiser");
                    types.Add("Heavy Cruiser");
                    types.Add("Large Cruiser");
                    types.Add("Munition");
                }
                else if (type.Equals("Main"))
                {
                    types.Add("Battleship");
                    types.Add("Battlecruiser");
                    types.Add("Aircraft Carrier");
                    types.Add("Light Carrier");
                    types.Add("Monitor");
                    types.Add("Aviation Battlship");
                    types.Add("Repair");
                }
                else
                {
                    types.Add("Monitor");
                    types.Add("Munition");
                    types.Add("Aviation Battlship");
                }
            }
            return types;
        }

        //--------------//
        //SEARCH BY NAME//
        //--------------//
        private void Search(object sender)
        {
            TextBox textBox = (TextBox)sender;
            SearchShip(textBox.Text);
            RaisePropertyChanged("FilterListStat");
        }


        //-------------------//
        //My Sorting Function//
        //-------------------//
        public void SortByLevel()
        {
            if (ShipDataLists != null && ShipDataLists.Count > 0)
            {
                FilterListStat = ShipDataLists.OrderByDescending(x => x.MyShip.Lv).ToList();
            }
        }

        public void SortByRarity()
        {
            List<ShipDataList> sortedList = new List<ShipDataList>();
            List<ShipDataList> CommonList = new List<ShipDataList>();
            List<ShipDataList> RareList = new List<ShipDataList>();
            List<ShipDataList> EliteList = new List<ShipDataList>();
            List<ShipDataList> SuperRareList = new List<ShipDataList>();
            List<ShipDataList> UltraRareList = new List<ShipDataList>();

            foreach (ShipDataList ship in ShipDataLists)
            {
                switch (ship.MyShip.Rarity)
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
            FilterListStat = sortedList;
        }

        public void SortByStat(string stat)
        {
            List<KeyValuePair<int, ShipDataList>> sortedList = new List<KeyValuePair<int, ShipDataList>>();
            List<ShipDataList> sort = new List<ShipDataList>();
            object valueObj;
            foreach (ShipDataList dataList in ShipDataLists)
            {
                valueObj = dataList.MyShip.CurrentStat.GetType().GetProperty(stat).GetValue(dataList.MyShip.CurrentStat);
                int.TryParse(valueObj.ToString(), out int valueInt);
                sortedList.Add(new KeyValuePair<int, ShipDataList>(valueInt, dataList));
            }
            sortedList = sortedList.OrderByDescending(x => x.Key).ToList();
            foreach (KeyValuePair<int, ShipDataList> ship in sortedList)
            {
                sort.Add(ship.Value);
            }
            FilterListStat = sort;
        }

        public void SortByIndication(List<string> indexes)
        {
            List<ShipDataList> sortedList = new List<ShipDataList>();
            foreach (ShipDataList ship in FilterListStat)
            {
                foreach (string type in indexes)
                {
                    if (ship.MyShip.HullType.Equals(type))
                    {
                        sortedList.Add(ship);
                        break;
                    }
                }
            }
            FilterListStat = sortedList;
        }

        public void SortByFaction(List<string> factions)
        {
            List<ShipDataList> sortedList = new List<ShipDataList>();
            foreach (ShipDataList ship in FilterListStat)
            {
                foreach (string faction in factions)
                {
                    if (ship.Ship.Nationality.Contains(faction))
                        sortedList.Add(ship);
                    else if (faction.Equals("FactionOther"))
                    {
                        if (!ship.Ship.Nationality.Contains("Eagle") &&
                            !ship.Ship.Nationality.Contains("Royal") &&
                            !ship.Ship.Nationality.Contains("Sakura") &&
                            !ship.Ship.Nationality.Contains("Iron") &&
                            !ship.Ship.Nationality.Contains("Dragon") &&
                            !ship.Ship.Nationality.Contains("Sardegna") &&
                            !ship.Ship.Nationality.Contains("Iris") &&
                            !ship.Ship.Nationality.Contains("Vichya") &&
                            !ship.Ship.Nationality.Contains("Northern"))
                            sortedList.Add(ship);
                    }
                }
            }
            FilterListStat = sortedList;
        }

        public void SortByRarity(List<string> rarities)
        {
            List<ShipDataList> sortedList = new List<ShipDataList>();
            foreach (ShipDataList ship in FilterListStat)
            {
                foreach (string rarity in rarities)
                {
                    if (ship.MyShip.Rarity.Contains(rarity))
                        sortedList.Add(ship);
                    else if (rarity.Equals("Super") && ship.MyShip.Rarity.Equals("Priority"))
                        sortedList.Add(ship);
                    else if (rarity.Equals("Ultra") && ship.MyShip.Rarity.Equals("Decisive"))
                        sortedList.Add(ship);
                }
            }
            FilterListStat = sortedList;
        }

        public void SearchShip(string name)
        {
            List<ShipDataList> sortedList = new List<ShipDataList>();
            string lowerCaseShip, lowerCaseName = name.ToLower();
            foreach (ShipDataList ship in ShipDataLists)
            {
                lowerCaseShip = ship.Ship.Name.ToLower();
                if (lowerCaseShip.Contains(lowerCaseName))
                    sortedList.Add(ship);
            }
            FilterListStat = sortedList;
        }
    }
}
