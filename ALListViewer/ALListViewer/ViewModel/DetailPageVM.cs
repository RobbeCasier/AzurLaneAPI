using ALListViewer.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ALListViewer.ViewModel
{
    class DetailPageVM : ObservableObject
    {
        public Brush ColorBrush { get; set; }
        private Skill _selectedSkill;
        public Skill SelectedSkill
        {
            get => _selectedSkill;
            set
            {
                _selectedSkill = value;
            }
        }
        private Ship _ship;
        public Ship Ship
        {
            get => _ship;
            set
            {
                _ship = value;

                Stats = GetStats();
                Skins = GetSkins();
                StatIndex = 0;
                SelectedStat = Stats[StatIndex];
                RaisePropertyChanged("Stats");
                RaisePropertyChanged("Skins");
                RaisePropertyChanged("StatIndex");
            }
        }

        public List<string> Stats { get; set; }
        public List<string> Skins { get; set; }
        public Skin CurrentSkin { get; set; }

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

        private string _selectedSkin;
        public string SelectedSkin
        {
            get => _selectedSkin;
            set
            {
                _selectedSkin = value;

                CurrentSkin = Ship.Skins[Skins.IndexOf(value)];
                RaisePropertyChanged("CurrentSkin");
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

        private List<string> GetSkins()
        {
            List<string> skins = new List<string>();

            if (Ship.Skins != null)
            {
                foreach (Skin skin in Ship.Skins)
                {
                    skins.Add(skin.Name);
                }
            }
            return skins;
        }

        public RelayCommand<object> CmdExpand
        {
            get => new RelayCommand<object>(Expand);
        }

        private void Expand(object expander)
        {
            Expander exp = expander as Expander;
            if (exp.IsExpanded)
            {
                exp.IsExpanded = false;
                var taskResult = Task.Run(() =>
                {
                    return Collapse(exp);
                });
                taskResult.ConfigureAwait(true).GetAwaiter().OnCompleted(
                    () =>
                    {
                        exp.IsExpanded = true;
                        RaisePropertyChanged("SelectedSkill");
                    });
            }
            else
            {
                exp.IsExpanded = true;
                RaisePropertyChanged("SelectedSkill");
            }
        }


        async private Task Collapse(Expander exp)
        {
            await Task.Delay((int)exp.AnimationLength);
        }
    }
}
