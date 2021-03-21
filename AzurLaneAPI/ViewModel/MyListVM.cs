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

namespace AzurLaneAPI.ViewModel
{
    class MyListVM : ObservableObject
    {
        public MainViewModel MainVM { get; set; }
        public AzurLaneRepositoryBASE _alRepository = new AzurLaneRepositoryWEB();



        public List<MyShip> MyList { get; set; } = new List<MyShip>();

        public ObservableCollection<ShipDataList> ShipDataLists { get; set; } = new ObservableCollection<ShipDataList>();

        public Ship _selectedShip;
        public Ship SelectedShip
        {
            get { return _selectedShip; }
            set { _selectedShip = value; }
        }

        public MyListVM()
        {
            GetMyList();
        }


        public void AddShip(Ship ship)
        {
            MyShip newShip = new MyShip
            {
                Id = ship.Id,
                CurrentStat = ship.Stats.BaseStats
            };
            MyList.Add(newShip);
            ShipDataLists.Add(new ShipDataList { MyShip = newShip, Ship = ship });
            RaisePropertyChanged("ShipDataLists");
            WriteJson();
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(Refresh);
            }
        }
        private void Refresh()
        {
            RaisePropertyChanged("ShipList");
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
                                foreach (MyShip myShip in MyList)
                                {
                                    Ship tShip = taskRes.Result.Find(x => x.Id == myShip.Id);
                                    ShipDataLists.Add(new ShipDataList { MyShip = myShip, Ship = tShip });
                                }
                                RaisePropertyChanged("ShipDataLists");
                            }
                        }
                    });
        }
    }
}
