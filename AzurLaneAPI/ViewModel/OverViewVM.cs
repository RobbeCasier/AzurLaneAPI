using AzurLaneAPI.Model;
using AzurLaneAPI.Repository;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AzurLaneAPI.ViewModel
{
    class OverViewVM : ObservableObject
    {
        //----------------------//
        //CHANGE REPOSITORY HERE//
        //----------------------//
        //// AzurLaneRepository+(type)
        //// local repository   => type = JSON
        //// web repository     => type = WEB
        public AzurLaneRepositoryBASE _alRepository = new AzurLaneRepositoryWEB();

        //ship lists
        public List<Ship> Ships { get; set; }

        //sorting variables
        private string SortingParameter { get; set; } = "cId";
        private List<string> SortingParametersIndex { get; set; } = new List<string>() { "All" };
        private List<string> SortingParametersFaction{ get; set; } = new List<string>() { "All" };
        private List<string> SortingParametersRarity{ get; set; } = new List<string>() { "All" };

        //Mainviewmodel
        public MainViewModel MainVM { get; set; }

        
        //selected ship
        private Ship _selectedShip;

        public Ship SelectedShip
        {
            get { return _selectedShip; }
            set 
            { 
                _selectedShip = value; 
                if (value != null)
                    MainVM.DetailOverviewSwitch(); 
            }
        }

        //constructor
        public OverViewVM()
        {
            GetAllShips();
        }

        public void SetMainVM(MainViewModel mainVM)
        {
            MainVM = mainVM;
        }

        //-------------//
        //RELAYCOMMANDS//
        //-------------//
        public RelayCommand<Ship> AddShipCommand
        {
            get
            {
                return new RelayCommand<Ship>(MainVM.AddShipToList);
            }
        }
        public RelayCommand GetAll
        {
            get
            {
                return new RelayCommand(GetAllShips);
            }
        }
        public RelayCommand GetCommon
        {
            get
            {
                return new RelayCommand(GetCommonShips);
            }
        }
        public RelayCommand GetCollabs
        {
            get
            {
                return new RelayCommand(GetCollabsShips);
            }
        }
        public RelayCommand GetPR
        {
            get
            {
                return new RelayCommand(GetPRShips);
            }
        }
        public RelayCommand GetMETA
        {
            get
            {
                return new RelayCommand(GetMETAShips);
            }
        }

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

        //------------//
        //MAIN SORTING//
        //------------//
        private void GetAllShips()
        {
            var taskRes = Task.Run(() =>
            {
                return _alRepository.GetShips();
            });
            taskRes.ConfigureAwait(true).GetAwaiter().OnCompleted(
                () =>
                {
                    Ships = taskRes.Result;
                    Filter();
                    RaisePropertyChanged("Ships");
                });
        }

        private void GetCommonShips()
        {
            Ships = _alRepository.GetCommonShips();
            Filter();
            RaisePropertyChanged("Ships");
        }
        private void GetCollabsShips()
        {
            Ships = _alRepository.GetCollabsShips();
            Filter();
            RaisePropertyChanged("Ships");
        }
        private void GetPRShips()
        {
            Ships = _alRepository.GetPRShips();
            Filter();
            RaisePropertyChanged("Ships");
        }
        private void GetMETAShips()
        {
            Ships = _alRepository.GetMETAShips();
            Filter();
            RaisePropertyChanged("Ships");
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
                SortingParameter = "cId";
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
                if (SortingParameter.Equals("cId"))
                    Ships = _alRepository.SortByIndex();
                else if (SortingParameter.Equals("cRarity"))
                    Ships = _alRepository.SortByRarity();
                else
                {
                    string stat = SortingParameter.Substring(1);
                    Ships = _alRepository.SortByStat(stat);
                }

            }
            if (!SortingParametersIndex[0].Equals("All"))
            {
                Ships = _alRepository.SortByIndication(GetterShipTypes());
            }
            if (!SortingParametersFaction[0].Equals("All"))
            {
                Ships = _alRepository.SortByFaction(SortingParametersFaction);
            }
            if (!SortingParametersRarity[0].Equals("All"))
            {
                Ships = _alRepository.SortByRarity(SortingParametersRarity);
            }
            RaisePropertyChanged("Ships");

        }

        private List<string> GetterShipTypes()
        {
            List<string> types = new List<string>();
            foreach(string type in SortingParametersIndex)
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
            Ships = _alRepository.SearchShip(textBox.Text);
            RaisePropertyChanged("Ships");
        }
    }
}
