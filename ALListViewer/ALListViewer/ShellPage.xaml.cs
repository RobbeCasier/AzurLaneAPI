using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ALListViewer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellPage : Xamarin.Forms.Shell
    {
        public ShellPage()
        {
            InitializeComponent();
        }
    }
}