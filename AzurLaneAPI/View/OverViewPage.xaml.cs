using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzurLaneAPI.View
{
    /// <summary>
    /// Interaction logic for OverViewPage.xaml
    /// </summary>
    public partial class OverViewPage : Page
    {
        private int Index { get; set; } = 1;
        private int Faction { get; set; } = 1;
        private int Rarity { get; set; } = 1;
        public OverViewPage()
        {
            InitializeComponent();
        }

        private void OnSelectionIndex(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Name.Equals("cRarity"))
            {
                cId.IsChecked = false;
                cFirepower.IsChecked = false;
                cAviation.IsChecked = false;
                cEvasion.IsChecked = false;
                cAntiair.IsChecked = false;
                cTorpedo.IsChecked = false;
                cReload.IsChecked = false;
                cHealth.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cId"))
            {
                cRarity.IsChecked = false;
                if (cFirepower != null)
                {
                    cFirepower.IsChecked = false;
                    cAviation.IsChecked = false;
                    cEvasion.IsChecked = false;
                    cAntiair.IsChecked = false;
                    cTorpedo.IsChecked = false;
                    cReload.IsChecked = false;
                    cHealth.IsChecked = false;
                    cAntisubmarineWarfare.IsChecked = false;
                }
            }
            else if (checkBox.Name.Equals("cFirepower"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cAviation.IsChecked = false;
                cEvasion.IsChecked = false;
                cAntiair.IsChecked = false;
                cTorpedo.IsChecked = false;
                cReload.IsChecked = false;
                cHealth.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cAviation"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cFirepower.IsChecked = false;
                cEvasion.IsChecked = false;
                cAntiair.IsChecked = false;
                cTorpedo.IsChecked = false;
                cReload.IsChecked = false;
                cHealth.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cEvasion"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cAviation.IsChecked = false;
                cFirepower.IsChecked = false;
                cAntiair.IsChecked = false;
                cTorpedo.IsChecked = false;
                cReload.IsChecked = false;
                cHealth.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cAntiair"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cAviation.IsChecked = false;
                cEvasion.IsChecked = false;
                cFirepower.IsChecked = false;
                cTorpedo.IsChecked = false;
                cReload.IsChecked = false;
                cHealth.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cTorpedo"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cAviation.IsChecked = false;
                cEvasion.IsChecked = false;
                cAntiair.IsChecked = false;
                cFirepower.IsChecked = false;
                cReload.IsChecked = false;
                cHealth.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cReload"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cAviation.IsChecked = false;
                cEvasion.IsChecked = false;
                cAntiair.IsChecked = false;
                cTorpedo.IsChecked = false;
                cFirepower.IsChecked = false;
                cHealth.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cHealth"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cAviation.IsChecked = false;
                cEvasion.IsChecked = false;
                cAntiair.IsChecked = false;
                cTorpedo.IsChecked = false;
                cReload.IsChecked = false;
                cFirepower.IsChecked = false;
                cAntisubmarineWarfare.IsChecked = false;
            }
            else if (checkBox.Name.Equals("cAntisubmarineWarfare"))
            {
                cRarity.IsChecked = false;
                cId.IsChecked = false;
                cAviation.IsChecked = false;
                cEvasion.IsChecked = false;
                cAntiair.IsChecked = false;
                cTorpedo.IsChecked = false;
                cReload.IsChecked = false;
                cHealth.IsChecked = false;
                cFirepower.IsChecked = false;
            }
        }

        private void OnUnselectionIndex(object sender, RoutedEventArgs e)
        {
            if (cRarity.IsChecked == false
                && cFirepower.IsChecked == false
                && cAviation.IsChecked == false
                && cEvasion.IsChecked == false
                && cAntiair.IsChecked == false
                && cTorpedo.IsChecked == false
                && cReload.IsChecked == false
                && cHealth.IsChecked == false
                && cAntisubmarineWarfare.IsChecked == false)
            cId.IsChecked = true;
        }

        private void OnIndexSelection(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Name.Equals("cIndexAll"))
            {
                if (cMain != null)
                {
                    cMain.IsChecked = false;
                    cVanguard.IsChecked = false;
                    cDD.IsChecked = false;
                    cCL.IsChecked = false;
                    cCA.IsChecked = false;
                    cBB.IsChecked = false;
                    cCV.IsChecked = false;
                    cRepair.IsChecked = false;
                    cSS.IsChecked = false;
                    cIndexOther.IsChecked = false;
                }
                Index = 1;
            }
            else if (checkBox.Name.Equals("cMain"))
            {
                cIndexAll.IsChecked = false;
                cVanguard.IsChecked = false;
                cDD.IsChecked = false;
                cCL.IsChecked = false;
                cCA.IsChecked = false;
                cBB.IsChecked = false;
                cCV.IsChecked = false;
                cRepair.IsChecked = false;
                cSS.IsChecked = false;
                cIndexOther.IsChecked = false;
                Index = 1;
            }
            else if (checkBox.Name.Equals("cVanguard"))
            {
                cIndexAll.IsChecked = false;
                cMain.IsChecked = false;
                cDD.IsChecked = false;
                cCL.IsChecked = false;
                cCA.IsChecked = false;
                cBB.IsChecked = false;
                cCV.IsChecked = false;
                cRepair.IsChecked = false;
                cSS.IsChecked = false;
                cIndexOther.IsChecked = false;
                Index = 1;
            }
            else
            {
                ++Index;
                cIndexAll.IsChecked = false;
                cMain.IsChecked = false;
                cVanguard.IsChecked = false;
            }
        }
        private void OnIndexUnselection(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Name.Equals("cMain") || checkBox.Name.Equals("cVanguard") || checkBox.Name.Equals("cIndexAll"))
            {
                if (Index <= 1)
                {
                    if (cVanguard.IsChecked == false && cMain.IsChecked == false)
                        cIndexAll.IsChecked = true;
                }
                else if (Index >= 1)
                    --Index;
            }
            else
            {
                --Index;
                if (Index == 0 && (cMain.IsChecked == false && cVanguard.IsChecked == false))
                    cIndexAll.IsChecked = true;
            }
        }

        private void OnFactionSelection(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Name.Equals("cAllFaction"))
            {
                if (cEagle != null)
                {
                    cEagle.IsChecked = false;
                    cRoyal.IsChecked = false;
                    cSakura.IsChecked = false;
                    cIron.IsChecked = false;
                    cDragon.IsChecked = false;
                    cSardegna.IsChecked = false;
                    cNorthern.IsChecked = false;
                    cIris.IsChecked = false;
                    cVichya.IsChecked = false;
                    cFactionOther.IsChecked = false;
                    Faction = 1;
                }
            }
            else
            {
                ++Faction;
                cAllFaction.IsChecked = false;
            }
        }

        private void OnFactionUnselection(object sender, RoutedEventArgs e)
        {
            --Faction;
            if (Faction < 1)
            {
                cAllFaction.IsChecked = true;
            }

        }

        private void OnRaritySelecion(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Name.Equals("cRarityAll"))
            {
                if (cNormal != null)
                {
                    cNormal.IsChecked = false;
                    cRare.IsChecked = false;
                    cElite.IsChecked = false;
                    cSuper.IsChecked = false;
                    cUltra.IsChecked = false;
                    Rarity = 1;
                }
            }
            else
            {
                ++Rarity;
                cRarityAll.IsChecked = false;
            }
        }

        private void OnRarityUnselection(object sender, RoutedEventArgs e)
        {
            --Rarity;
            if (Rarity < 1)
            {
                cRarityAll.IsChecked = true;
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                searchButton.Command.Execute(sender);
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            myList.Items.Refresh();
        }
    }
}
