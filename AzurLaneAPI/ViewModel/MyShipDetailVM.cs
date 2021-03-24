using AzurLaneAPI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AzurLaneAPI.ViewModel
{
    class MyShipDetailVM : ViewModelBase
    {
        //SKINS
        public Skin CurrentSkin { get; set; }
        public List<string> MySkins { get; set; } = new List<string>();
        public List<string> Skins { get; set; }
        private string _selectedSkin;

        public string SelectedSkin
        {
            get { return _selectedSkin; }
            set 
            {
                if (value != null)
                {
                    _selectedSkin = value;
                    CurrentSkin = CurrentShip.Ship.Skins[Skins.IndexOf(value)];
                    RaisePropertyChanged("CurrentSkin");
                    RaisePropertyChanged("IsUnlocked");
                    RaisePropertyChanged("IsNotUnlocked");
                }
            }
        }
        private List<string> GetSkins()
        {
            List<string> skins = new List<string>();

            if (CurrentShip.Ship.Skins != null)
            {
                foreach (Skin skin in CurrentShip.Ship.Skins)
                {
                    skins.Add(skin.Name);
                }
            }
            return skins;
        }

        public Visibility IsNotUnlocked
        {
            get
            {
                //if skin name exist in list, not visible
                foreach (string skinName in MySkins)
                {
                    if (CurrentSkin.Name == skinName)
                        return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        public Visibility IsUnlocked
        {
            get
            {
                //if skin name exist in list, not visible
                foreach (string skinName in MySkins)
                {
                    if (CurrentSkin.Name == skinName && CurrentSkin.Name != CurrentShip.Ship.Skins[CurrentShip.MyShip.CurrentSkin].Name)
                        return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public Basestats CurrentStat { get; set; }
        public List<ShipDataList> Ships { get; set; }
        private ShipDataList _currentShip;
        public MainViewModel MainVM { get; set; }
        private double AffMultiplier { get; set; }

        public ShipDataList CurrentShip
        {
            get { return _currentShip; }
            set 
            { 
                _currentShip = value;
                RaisePropertyChanged("CurrentShip");
                Skins = GetSkins();
                RaisePropertyChanged("Skins");
                SelectedSkin = value.Ship.Skins[value.MyShip.CurrentSkin].Name;
                RaisePropertyChanged("SelectedSkin");
                List<string> newList = new List<string>();
                foreach (ushort element in _currentShip.MyShip.MySkins)
                {
                    newList.Add(_currentShip.Ship.Skins[element].Name);
                }
                MySkins = newList;
                RaisePropertyChanged("MySkins");
                RaisePropertyChanged("IsUnlocked");
                RaisePropertyChanged("IsNotUnlocked");
                CurrentStat = value.MyShip.CurrentStat;
                RaisePropertyChanged("CurrentStat");
                SetCurrentStats();
            }
        }

        //CHOICEBOX
        private bool _isMarried;
        private bool _isRetrofitted;
        public bool IsMarried
        {
            get { return _isMarried; }
            set 
            { 
                _isMarried = value;
                RaisePropertyChanged("IsMarried");
                RaisePropertyChanged("MaxAffection");
            }
        }

        public bool IsRetrofitted
        {
            get { return _isRetrofitted; }
            set
            {
                _isRetrofitted = CurrentShip.MyShip.IsRetrofitted = value;
                RaisePropertyChanged("IsRetrofitted");
                CalcMaxStat();
                if (MySkins.Find(x => x.Equals("Retrofit")) == null && value == true)
                {
                    MySkins.Add(CurrentShip.Ship.Skins[1].Name);
                    SelectedSkin = CurrentShip.Ship.Skins[1].Name;
                    CurrentShip.MyShip.CurrentSkin = 1;
                    RaisePropertyChanged("SelectedSkin");
                    RaisePropertyChanged("MySkins");
                    List<ushort> newList = CurrentShip.MyShip.MySkins.ToList();
                    newList.Add(1);
                    CurrentShip.MyShip.MySkins = newList.ToArray();
                    CurrentShip.MyShip.Rarity = GetNextRarity(CurrentShip.MyShip.Rarity);
                    CurrentShip.MyShip.HullType = CurrentShip.Ship.RetrofitHullType;
                    RaisePropertyChanged("IsUnlocked");
                    RaisePropertyChanged("IsNotUnlocked");

                }
                MainVM.UpdateMyShipList();
                RaisePropertyChanged("CurrentShip");
            }
        }

        private string GetNextRarity(string current)
        {
            switch (current)
            {
                case "Normal":
                    return "Rare";
                case "Rare":
                    return "Elite";
                case "Elite":
                    return "Super Rare";
                case "Super Rare":
                    return "Ultra Rare";
                default:
                    break;
            }
            return "";
        }

        //SLIDERS
        private ushort _affProgress;
        private ushort _hpProgress;
        private ushort _fpProgress;
        private ushort _aaProgress;
        private ushort _aswProgress;
        private ushort _trpProgress;
        private ushort _aviProgress;
        private ushort _rldProgress;
        private ushort _evaProgress;
        private ushort _costProgress;
        private ushort _oxyProgress;
        private ushort _amoProgress;
        private ushort _lvProgress;

        public ushort AffProgress 
        { 
            get
            {
                return _affProgress;
            }
            set 
            {
                _affProgress = value;
                CalcAffection(); 
            } 
        }
        public string AffProgressString { get;set; }
        public ushort HPProgress
        {
            get { return _hpProgress; }
            set 
            { 
                _hpProgress = value; 
                RaisePropertyChanged("HPProgress");
            }
        }
        public ushort FPProgress
        {
            get { return _fpProgress; }
            set
            {
                _fpProgress = value;
                RaisePropertyChanged("FPProgress");
            }
        }
        public ushort AAProgress
        {
            get { return _aaProgress; }
            set
            {
                _aaProgress = value;
                RaisePropertyChanged("AAProgress");
            }
        }
        public ushort ASWProgress
        {
            get { return _aswProgress; }
            set
            {
                _aswProgress = value;
                RaisePropertyChanged("ASWProgress");
            }
        }
        public ushort TRPProgress
        {
            get { return _trpProgress; }
            set
            {
                _trpProgress = value;
                RaisePropertyChanged("TRPProgress");
            }
        }
        public ushort AVIProgress
        {
            get { return _aviProgress; }
            set
            {
                _aviProgress = value;
                RaisePropertyChanged("AVIProgress");
            }
        }
        public ushort RLDProgress
        {
            get { return _rldProgress; }
            set
            {
                _rldProgress = value;
                RaisePropertyChanged("RLDProgress");
            }
        }
        public ushort EVAProgress
        {
            get { return _evaProgress; }
            set
            {
                _evaProgress = value;
                RaisePropertyChanged("EVAProgress");
            }
        }
        public ushort CostProgress
        {
            get { return _costProgress; }
            set
            {
                _costProgress = value;
                RaisePropertyChanged("CostProgress");
            }
        }
        public ushort OXYProgress
        {
            get { return _oxyProgress; }
            set
            {
                _oxyProgress = value;
                RaisePropertyChanged("OXYProgress");
            }
        }
        public ushort AMOProgress
        {
            get { return _amoProgress; }
            set
            {
                _amoProgress = value;
                RaisePropertyChanged("AMOProgress");
            }
        }

        public ushort LVProgress
        { 
            get { return _lvProgress; }
            set
            {
                _lvProgress = value;
                RaisePropertyChanged("LVProgress");
            }
        }


        //Current max stats
        public Basestats CurrentMaxStat
        {
            get; set;
        }

        private void CalcMaxStat()
        {
            if (CurrentShip.Ship != null)
            {
                Basestats stat120 = CurrentShip.Ship.Stats.Level120;
                Basestats stat120Retro = CurrentShip.Ship.Stats.Level120Retrofit;
                Basestats stat = new Basestats();

                if (IsRetrofitted)
                {
                    stat.Health = CalcMaxStatAtAffection(stat120.Health, stat120Retro.Health);
                    stat.Firepower = CalcMaxStatAtAffection(stat120.Firepower, stat120Retro.Firepower);
                    stat.Antiair = CalcMaxStatAtAffection(stat120.Antiair, stat120Retro.Antiair);
                    stat.AntisubmarineWarfare = CalcMaxStatAtAffection(stat120.AntisubmarineWarfare, stat120Retro.AntisubmarineWarfare);
                    stat.Oxygen = CalcMaxStatAtAffection(stat120.Oxygen, stat120Retro.Oxygen);
                    stat.Torpedo = CalcMaxStatAtAffection(stat120.Torpedo, stat120Retro.Torpedo);
                    stat.Aviation = CalcMaxStatAtAffection(stat120.Aviation, stat120Retro.Aviation);
                    stat.Ammunition = CalcMaxStatAtAffection(stat120.Ammunition, stat120Retro.Ammunition);
                    stat.Reload = CalcMaxStatAtAffection(stat120.Reload, stat120Retro.Reload);
                    stat.Evasion = CalcMaxStatAtAffection(stat120.Evasion, stat120Retro.Evasion);
                    stat.OilConsumption = stat120Retro.OilConsumption;
                }
                else
                {
                    stat.Health = CalcMaxStatAtAffection(stat120.Health);
                    stat.Firepower = CalcMaxStatAtAffection(stat120.Firepower);
                    stat.Antiair = CalcMaxStatAtAffection(stat120.Antiair);
                    stat.AntisubmarineWarfare = CalcMaxStatAtAffection(stat120.AntisubmarineWarfare);
                    stat.Oxygen = CalcMaxStatAtAffection(stat120.Oxygen);
                    stat.Torpedo = CalcMaxStatAtAffection(stat120.Torpedo);
                    stat.Aviation = CalcMaxStatAtAffection(stat120.Aviation);
                    stat.Ammunition = CalcMaxStatAtAffection(stat120.Ammunition);
                    stat.Reload = CalcMaxStatAtAffection(stat120.Reload);
                    stat.Evasion = CalcMaxStatAtAffection(stat120.Evasion);
                    stat.OilConsumption = stat120.OilConsumption;
                }
                CurrentMaxStat = stat;
            }
            else
                CurrentMaxStat = new Basestats();
            RaisePropertyChanged("CurrentMaxStat");
        }
        private void CalcAffection()
        {
            AffMultiplier = 1.0;
            if (AffProgress <= 30)
                AffProgressString = "Disappointed";
            else if (AffProgress <= 60)
                AffProgressString = "Stranger";
            else if (AffProgress <= 80)
            {
                AffProgressString = "Friendly";
                AffMultiplier = 1.01;
            }
            else if (AffProgress <= 99)
            {
                AffProgressString = "Crush";
                AffMultiplier = 1.03;
            }
            else if (AffProgress == 100 && !CurrentShip.MyShip.IsMarried)
            {
                AffProgressString = "Love";
                AffMultiplier = 1.06;
            }
            else
            {
                AffProgressString = "Oath";
                if (AffProgress < 200)
                    AffMultiplier = 1.06;
                AffMultiplier = 1.12;
            }
            RaisePropertyChanged("AffProgressString");
            RaisePropertyChanged("AffProgress");
        }

        public ushort MaxAffection
        {
            get
            {
                if (IsMarried)
                    return 200;
                return 100;
            }
        }


        public RelayCommand SwitchBack
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        public RelayCommand UnlockCommand
        {
            get
            {
                return new RelayCommand(Unlock);
            }
        }

        public RelayCommand ActivateCommand
        {
            get
            {
                return new RelayCommand(Activate);
            }
        }

        private void Activate()
        {
            CurrentShip.MyShip.CurrentSkin = (ushort)Skins.IndexOf(CurrentSkin.Name);
            RaisePropertyChanged("IsUnlocked");

            MainVM.UpdateMyShipList();
        }

        private void Unlock()
        {
            ushort id = (ushort)Skins.IndexOf(CurrentSkin.Name);
            CurrentShip.MyShip.CurrentSkin = id;

            List<ushort> newList = CurrentShip.MyShip.MySkins.ToList();
            newList.Add(id);
            CurrentShip.MyShip.MySkins = newList.ToArray();
            RaisePropertyChanged("CurrentShip.MyShip.MySkins");
            RaisePropertyChanged("IsUnlocked");
            RaisePropertyChanged("IsNotUnlocked");

            MainVM.UpdateMyShipList();
        }

        private void Switch()
        {
            MainVM.DetailMyListSwitch();
        }
        private void Save()
        {
            CurrentShip.MyShip.CurrentAffection = AffProgress;
            CurrentShip.MyShip.IsMarried = IsMarried;
            CurrentShip.MyShip.IsRetrofitted = IsRetrofitted;

            CurrentShip.MyShip.CurrentStat.Health = HPProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Firepower = FPProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Antiair = AAProgress.ToString();
            CurrentShip.MyShip.CurrentStat.AntisubmarineWarfare = ASWProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Oxygen = OXYProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Torpedo = TRPProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Aviation = AVIProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Ammunition = AMOProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Reload = RLDProgress.ToString();
            CurrentShip.MyShip.CurrentStat.Evasion = EVAProgress.ToString();
            CurrentShip.MyShip.CurrentStat.OilConsumption = CostProgress.ToString();
            CurrentShip.MyShip.Lv = LVProgress;

            MainVM.UpdateMyShipList();
        }

        private void SetCurrentStats()
        {
            AffProgress = CurrentShip.MyShip.CurrentAffection;
            RaisePropertyChanged("AffProgress");
            HPProgress = ushort.Parse(CurrentStat.Health);
            FPProgress = ushort.Parse(CurrentStat.Firepower);
            AAProgress = ushort.Parse(CurrentStat.Antiair);
            ASWProgress = ushort.Parse(CurrentStat.AntisubmarineWarfare);
            if (CurrentStat.Oxygen != null)
                OXYProgress = ushort.Parse(CurrentStat.Oxygen);
            TRPProgress = ushort.Parse(CurrentStat.Torpedo);
            AVIProgress = ushort.Parse(CurrentStat.Aviation);
            if (CurrentStat.Ammunition != null)
                AMOProgress = ushort.Parse(CurrentStat.Ammunition);
            RLDProgress = ushort.Parse(CurrentStat.Reload);
            EVAProgress = ushort.Parse(CurrentStat.Evasion);
            CostProgress = ushort.Parse(CurrentStat.OilConsumption);
            LVProgress = CurrentShip.MyShip.Lv;
            IsMarried = CurrentShip.MyShip.IsMarried;
            IsRetrofitted = CurrentShip.MyShip.IsRetrofitted;
        }

        private string CalcMaxStatAtAffection(string stat, string retStat = "")
        {
            if (retStat == null || retStat.Equals("0"))
                return stat;
            else if (retStat.Equals(""))
            {
                if (stat == null)
                    return stat;
                else if (stat.Equals("0"))
                    return stat;
                double stat120 = ushort.Parse(stat);
                double tempInt = (ushort)(stat120 / 1.06);
                tempInt *= AffMultiplier;
                return ((ushort)tempInt).ToString();
            }
            else
            {
                double stat120 = ushort.Parse(stat);
                double stat120Retro = ushort.Parse(retStat);
                double diff = stat120Retro - stat120;
                double tempInt = (ushort)(stat120 / 1.06);
                tempInt *= AffMultiplier;
                tempInt += diff;
                return ((ushort)tempInt).ToString();
            }
        }


    }
}
