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
using ALListViewer.View;

namespace ALListViewer.ViewModel
{
    class BaseSort
    {
        public string Name { get; set; }
        public Action Function { get; set; }
    }
    class MainVM : ObservableObject
    {
        public ALRepository _alRepository = new ALRepository();

        public List<Ship> Ships { get; set; }
        public List<BaseSort> BasicSortList { get; set; }

        public MainVM()
        {
            SetupBasicSortingTypes();
            GetShips();
        }

        //selection here
        private BaseSort _selectedBaseSort;
        public BaseSort SelectedBaseSort
        {
            get
            {
                return _selectedBaseSort;
            }
            set
            {
                if (Ships != null)
                {
                    _selectedBaseSort = value;
                    _selectedBaseSort.Function();
                }
            }
        }

        private Ship _selectedShip;
        public Ship SelectedShip
        {
            get
            {
                return _selectedShip;
            }
            set
            {
                _selectedShip = value;
                GoToDetailPage();
            }
        }

        async void GoToDetailPage()
        {
            var detailPage = new DetailPage();
            Brush bgColor = GetColorFromShipRarity(SelectedShip.Rarity);
            detailPage.Title = SelectedShip.Name;
            detailPage.BarBackground = bgColor;
            await Application.Current.MainPage.Navigation.PushAsync(detailPage, true);
            (detailPage.BindingContext as DetailPageVM).Ship = SelectedShip;
            (detailPage.BindingContext as DetailPageVM).ColorBrush = bgColor;
            (detailPage.BindingContext as DetailPageVM).RaisePropertyChanged("Ship");
            (detailPage.BindingContext as DetailPageVM).RaisePropertyChanged("ColorBrush");
        }
        private void SetupBasicSortingTypes()
        {
            BasicSortList = new List<BaseSort>();
            BaseSort sort = new BaseSort
            {
                Name = "All",
                Function = GetShips
            };
            BasicSortList.Add(sort);

            sort = new BaseSort
            {
                Name = "Common",
                Function = GetCommonShips
            };
            BasicSortList.Add(sort);

            sort = new BaseSort
            {
                Name = "Collabs",
                Function = GetCollabsShips
            };
            BasicSortList.Add(sort);

            sort = new BaseSort
            {
                Name = "Priority Ship",
                Function = GetPRShips
            };
            BasicSortList.Add(sort);

            sort = new BaseSort
            {
                Name = "META",
                Function = GetMETAShips
            };
            BasicSortList.Add(sort);

            RaisePropertyChanged("BasicSortList");
        }

        public ICommand CmdSearch => new Command<string>((string value) =>
        {
            Ships = _alRepository.SearchShip(value);
            RaisePropertyChanged("Ships");
        });

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
                    var voiceTask = Task.Run(() =>
                    {
                        return _alRepository.GetVoices();
                    });
                    taskResult.ConfigureAwait(true).GetAwaiter().OnCompleted(
                        () =>
                        {
                            RaisePropertyChanged("Ships");
                        });
                });
        }

        private void GetCommonShips()
        {
            Ships = _alRepository.GetCommonShips();
            RaisePropertyChanged("Ships");
        }

        private void GetCollabsShips()
        {
            Ships = _alRepository.GetCollabsShips();
            RaisePropertyChanged("Ships");
        }

        private void GetPRShips()
        {
            Ships = _alRepository.GetPRShips();
            RaisePropertyChanged("Ships");
        }

        private void GetMETAShips()
        {
            Ships = _alRepository.GetMETAShips();
            RaisePropertyChanged("Ships");
        }

        ColorTypeConverter colorConverter = new ColorTypeConverter();
        private Brush GetColorFromShipRarity(string ShipRarity)
        {
            switch (ShipRarity)
            {
                case "Normal":
                    return new SolidColorBrush(Color.LightGray);
                case "Rare":
                    return new SolidColorBrush((Color)colorConverter.ConvertFromInvariantString("#9fe8ff"));
                case "Elite":
                    return new SolidColorBrush((Color)colorConverter.ConvertFromInvariantString("#c4adff"));
                case "Super Rare":
                case "Priority":
                    return new SolidColorBrush((Color)colorConverter.ConvertFromInvariantString("#ee9"));
                case "Ultra Rare":
                case "Decisive":
                    var gradStopCollection = new GradientStopCollection();
                    Color tempColor = (Color)colorConverter.ConvertFromInvariantString("#fbffca");
                    gradStopCollection.Add(new GradientStop(tempColor, 0.0f));
                    tempColor = (Color)colorConverter.ConvertFromInvariantString("#baffbf");
                    gradStopCollection.Add(new GradientStop(tempColor, 0.33f));
                    tempColor = (Color)colorConverter.ConvertFromInvariantString("#a7efff");
                    gradStopCollection.Add(new GradientStop(tempColor, 0.66f));
                    tempColor = (Color)colorConverter.ConvertFromInvariantString("#ffabff");
                    gradStopCollection.Add(new GradientStop(tempColor, 1.0f));
                    return new LinearGradientBrush(gradStopCollection, new Point(0, 0), new Point(1, 1));
                default:
                    return null;
            }
        }
    }
}