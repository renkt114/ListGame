using ListSLG.logic.generate;
using ListSLG.logic.growth;
using ListSLG.model;
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
using System.Windows.Shapes;

namespace ListSLG.window.debug
{
    /// <summary>
    /// GenerateDebugWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class GenerateDebugWindow : Window
    {
        private General newGeneral;

        public GenerateDebugWindow()
        {
            InitializeComponent();
        }

        private void Generate_Button_Click(object sender, RoutedEventArgs e)
        {

            General bCIgeneral = new General();

            bCIgeneral.attackAbility = int.Parse(bAtcNum.Text);
            bCIgeneral.defenseAbility = int.Parse(bDefNum.Text);
            bCIgeneral.maneuverAbility = int.Parse(bManNum.Text);

            bCIgeneral.attackGrowth = int.Parse(bAtcGrowthNum.Text);
            bCIgeneral.defenseGrowth = int.Parse(bDefGrowthNum.Text);
            bCIgeneral.maneuverGrowth = int.Parse(bManGrowthNum.Text);


            General pCIgeneral = new General();

            pCIgeneral.attackAbility = int.Parse(pAtcNum.Text);
            pCIgeneral.defenseAbility = int.Parse(pDefNum.Text);
            pCIgeneral.maneuverAbility = int.Parse(pManNum.Text);

            pCIgeneral.attackGrowth = int.Parse(pAtcGrowthNum.Text);
            pCIgeneral.defenseGrowth = int.Parse(pDefGrowthNum.Text);
            pCIgeneral.maneuverGrowth = int.Parse(pManGrowthNum.Text);


            BirthGeneralLogic birthGeneralLogic = new BirthGeneralLogic();


            this.newGeneral = birthGeneralLogic.BirthGeneral(pCIgeneral,bCIgeneral);

            // パラメータ表示
            gAtcNum.Content = newGeneral.attackAbility.ToString();
            gDefNum.Content = newGeneral.defenseAbility.ToString();
            gManNum.Content = newGeneral.maneuverAbility.ToString();

            gAtcGrowthNum.Content = newGeneral.attackGrowth.ToString();
            gDefGrowthNum.Content = newGeneral.defenseGrowth.ToString();
            gManGrowthNum.Content = newGeneral.maneuverGrowth.ToString();




        }

        private void Growth_Button_Click(object sender, RoutedEventArgs e)
        {

            GrowthGeneral.turnGrowthGeneral(newGeneral,true);

            // パラメータ表示
            gAtcNum.Content = newGeneral.attackAbility.ToString();
            gDefNum.Content = newGeneral.defenseAbility.ToString();
            gManNum.Content = newGeneral.maneuverAbility.ToString();

            gAtcGrowthNum.Content = newGeneral.attackGrowth.ToString();
            gDefGrowthNum.Content = newGeneral.defenseGrowth.ToString();
            gManGrowthNum.Content = newGeneral.maneuverGrowth.ToString();

            gAtcFromNowNum.Content = newGeneral.attackFromNow.ToString();
            gDefFromNowNum.Content = newGeneral.defenseFromNow.ToString();
            gManFromNowNum.Content = newGeneral.maneuverFromNow.ToString();





        }
    }
}
