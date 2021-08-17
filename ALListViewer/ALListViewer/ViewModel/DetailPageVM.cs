using ALListViewer.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALListViewer.ViewModel
{
    class DetailPageVM : ObservableObject
    {
        private Ship _ship;
        public Ship Ship 
        {
            get
            {
                return _ship;
            }
            set
            {
                _ship = value;
                RaisePropertyChanged("Ship");
            }
        }
    }
}
