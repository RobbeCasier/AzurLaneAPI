using AzurLaneAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurLaneAPI.ViewModel
{
    class MyListVM
    {
        public MainViewModel MainVM { get; set; }

        public List<Ship> MyList { get; set; }
    }
}
