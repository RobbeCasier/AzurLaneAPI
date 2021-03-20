using AzurLaneAPI.Model;
using AzurLaneAPI.Repository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzurLaneAPI.ViewModel
{
    class DetailVMShip : ViewModelBase
    {
        public List<string> Skins { get; set; }
        public List<string> Stats { get; set; }
        public Skin CurrentSkin { get; set; }
        public Basestats CurrentStat { get; set; }

        public List<Ship> Ships { get; set; }

        private Ship _currentShip;
        public Ship CurrentShip 
        { 
            get 
            { 
                return _currentShip; 
            } 
            set
            {
                _currentShip = value;

                Skins = GetSkins();
                Stats = GetStats();
                SelectedSkin = CurrentShip.Skins[0].Name;
                SelectedStat = "Base Stats";
                RaisePropertyChanged("SelectedStat");
                RaisePropertyChanged("SelectedSKin");
                RaisePropertyChanged("Skins");
                RaisePropertyChanged("Stats");
            }
        }

        private string _selectedSkin;
        public string SelectedSkin
        {
            get { return _selectedSkin; }
            set 
            {
                _selectedSkin = value;

                CurrentSkin = CurrentShip.Skins[Skins.IndexOf(value)];
                RaisePropertyChanged("CurrentSkin");
            }
        }

        private string _selectedStat;
        public string SelectedStat
        {
            get { return _selectedStat; }
            set
            {
                _selectedStat = value;

                string cObject = Regex.Replace(value, @"\s", "");
                CurrentStat = CurrentShip.Stats.GetStats(cObject);
                RaisePropertyChanged("CurrentStat");
            }
        }

        public MainViewModel MainVM { get; set; }

        public RelayCommand SwitchBack
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        private void Switch()
        {
            MainVM.DetailOverviewSwitch();
        }

        private List<string> GetSkins()
        {
            List<string> skins = new List<string>();

            if (CurrentShip.Skins != null)
            {
                foreach (Skin skin in CurrentShip.Skins)
                {
                    skins.Add(skin.Name);
                }
            }
            return skins;
        }

        private List<string> GetStats()
        {
            List<string> stats = new List<string>();

            if (CurrentShip.Stats != null)
            {
                stats.Add("Base Stats");
                stats.Add("Level 100");
                if (CurrentShip.Stats.Level100Retrofit != null)
                    stats.Add("Level 100 Retrofit");
                stats.Add("Level 120");
                if (CurrentShip.Stats.Level120Retrofit != null)
                    stats.Add("Level 120 Retrofit");
            }
            return stats;
        }

        public RelayCommand NextCommand
        {
            get
            {
                return new RelayCommand(Next);
            }
        }

        public RelayCommand PreviousCommand
        {
            get
            {
                return new RelayCommand(Previous);
            }
        }

        private void Next()
        {
            int index = Ships.FindIndex(x => x.Id == _currentShip.Id);
            if (index + 1 < Ships.Count)
                CurrentShip = Ships[index + 1];
            else
                CurrentShip = Ships[0];
            RaisePropertyChanged("CurrentShip");
        }

        private void Previous()
        {
            int index = Ships.FindIndex(x => x.Id == _currentShip.Id);
            if (index - 1 >= 0)
                CurrentShip = Ships[index - 1];
            else
                CurrentShip = Ships[Ships.Count - 1];
            RaisePropertyChanged("CurrentShip");
        }
    }
}
