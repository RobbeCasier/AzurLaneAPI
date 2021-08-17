using ALListViewer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ALListViewer.ViewModel
{
    class DetailPageVM : ObservableObject
    {
        private Ship _ship;
        public Ship Ship
        {
            get => _ship;
            set
            {
                _ship = value;

                Stats = GetStats();
                StatIndex = 0;
                SelectedStat = Stats[StatIndex];
                RaisePropertyChanged("Stats");
                RaisePropertyChanged("StatIndex");
            }
        }

        public List<string> Stats { get; set; }

        public Basestats CurrentStat { get; set; }

        public int StatIndex { get; set; }
        private string _selectedStat;
        public string SelectedStat
        {
            get => _selectedStat;
            set
            {
                _selectedStat = value;

                //replace the space in the text, this is the same as the variable name
                string temp = Regex.Replace(value, @"\s", "");
                CurrentStat = Ship.Stats.GetStats(temp);
                RaisePropertyChanged("CurrentStat");
            }
        }

        private List<string> GetStats()
        {
            List<string> stats = new List<string>();

            if (Ship.Stats != null)
            {
                stats.Add("Base Stats");
                stats.Add("Level 100");
                if (Ship.Stats.Level100Retrofit != null)
                {
                    stats.Add("Level 100 Retrofit");
                }

                stats.Add("Level 120");
                if (Ship.Stats.Level120Retrofit != null)
                {
                    stats.Add("Level 120 Retrofit");
                }
            }
            return stats;
        }
    }
}
