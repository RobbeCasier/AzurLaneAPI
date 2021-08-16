using ALListViewer.Repository;
using ALListViewer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Command;

namespace ALListViewer.ViewModel
{
    class MainVM : ObservableObject
    {
        public ALRepository _alRepository = new ALRepository();

        public List<Ship> Ships { get; set; }

        public MainVM()
        {
            GetShips();
        }


        public RelayCommand<string> CmdSearch
        {
            get
            {
                return new RelayCommand<string>(Search);
            }
        }

        private void Search(string sender)
        {
            Ships = _alRepository.SearchShip((string)sender);
            RaisePropertyChanged("Ships");
        }
        private void GetShips()
        {
            var taskResult = Task.Run(() =>
            {
                return _alRepository.GetShips();
            });
            taskResult.ConfigureAwait(true).GetAwaiter().OnCompleted(
                () =>
                {
                    Ships = taskResult.Result;
                    RaisePropertyChanged("Ships");
                });
        }
    }
}