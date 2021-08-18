using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALListViewer.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : TabbedPage
    {
        public DetailPage()
        {
            InitializeComponent();

            var genPage = new GeneralPage();
            var quotesPage = new QuotesPage();
            var galPage = new GalleryPage();
            genPage.BindingContext = BindingContext;
            quotesPage.BindingContext = BindingContext;
            galPage.BindingContext = BindingContext;
            Children.Add(genPage);
            Children.Add(quotesPage);
            Children.Add(galPage);
        }
    }
}