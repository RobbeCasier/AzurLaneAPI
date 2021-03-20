using AzurLaneAPI.Model;
using AzurLaneAPI.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AzurLaneAPI.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        public OverViewPage MainPage { get; set; } = new OverViewPage();
        public DetailPage AzurPage { get; set; } = new DetailPage();
        public MyListOverviewPage ListPage { get; set; } = new MyListOverviewPage();
        public Page CurrentPage { get; set; }

        public MainViewModel()
        {
            (MainPage.DataContext as OverViewVM).SetMainVM(this);
            (AzurPage.DataContext as DetailVMShip).MainVM = this;
            (ListPage.DataContext as MyListVM).MainVM = this;
            CurrentPage = MainPage;
        }

        public void DetailOverviewSwitch()
        {
            if (CurrentPage is OverViewPage)
            {
                Ship ship = (MainPage.DataContext as OverViewVM).SelectedShip;
                List<Ship> ships = (MainPage.DataContext as OverViewVM).Ships;
                if (ship == null)
                    return;

                (AzurPage.DataContext as DetailVMShip).CurrentShip = ship;
                (AzurPage.DataContext as DetailVMShip).Ships = ships;
                CurrentPage = AzurPage;
                RaisePropertyChanged("CurrentPage");
                (AzurPage.DataContext as DetailVMShip).RaisePropertyChanged("CurrentShip");
                (AzurPage.DataContext as DetailVMShip).RaisePropertyChanged("CurrentSkin");
                (AzurPage.DataContext as DetailVMShip).RaisePropertyChanged("Ships");
            }
            else
            {
                CurrentPage = MainPage;
                (MainPage.DataContext as OverViewVM).SelectedShip = null;
                (MainPage.DataContext as OverViewVM).RaisePropertyChanged("SelectedShip");
                RaisePropertyChanged("CurrentPage");
            }
        }

        public RelayCommand GoHome
        {
            get
            {
                return new RelayCommand(Home);
            }
        }

        public void Home()
        {
            CurrentPage = MainPage;
            RaisePropertyChanged("CurrentPage");
        }

        public RelayCommand ToMyList
        {
            get
            {
                return new RelayCommand(MyList);
            }
        }
        public void MyList()
        {
            CurrentPage = ListPage;
            RaisePropertyChanged("CurrentPage");
        }
    }
}
