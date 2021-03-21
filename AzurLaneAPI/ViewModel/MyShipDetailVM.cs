using AzurLaneAPI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurLaneAPI.ViewModel
{
    class MyShipDetailVM : ViewModelBase
    {
        public Skin CurrentSkin { get; set; }
        public Basestats CurrentStat { get; set; }
        public List<ShipDataList> Ships { get; set; }
        private ShipDataList _currentShip;
        public MainViewModel MainVM { get; set; }

        public ShipDataList CurrentShip
        {
            get { return _currentShip; }
            set 
            { 
                _currentShip = value;
                CurrentSkin = value.Ship.Skins[value.MyShip.CurrentSkin];
                CurrentStat = value.MyShip.CurrentStat;
                RaisePropertyChanged("CurrentSkin");
                RaisePropertyChanged("CurrentStat");
            }
        }

        public RelayCommand SwitchBack
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        private void Switch()
        {
            MainVM.DetailMyListSwitch();
        }

    }
}
