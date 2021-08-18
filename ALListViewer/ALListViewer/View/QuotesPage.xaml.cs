using MediaManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALListViewer.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuotesPage : ContentPage
    {
        public QuotesPage()
        {
            InitializeComponent();

            CmdPlayAudio = new Command<object>(async (object link) =>
            {
                if (link == null)
                    return;
                await CrossMediaManager.Current.Play((string)link);
            });

        }

        public ICommand CmdPlayAudio { get; set; }
    }
}