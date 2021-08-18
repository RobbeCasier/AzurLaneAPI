using MediaManager;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALListViewer
{
    public partial class App : Application
    {
        public App()
        {
            Xamarin.Forms.DataGrid.DataGridComponent.Init();
            CrossMediaManager.Current.Init();
            InitializeComponent();

            MainPage = new ShellPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
