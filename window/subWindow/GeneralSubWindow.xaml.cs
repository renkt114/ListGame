using ListSLG.dao;
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

namespace ListSLG.window.subWindow
{
    /// <summary>
    /// GeneralSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class GeneralSubWindow : Window
    {
        public GeneralSubWindow(General general)
        {

            InitializeComponent();
            showPedigreeChart(general.generalId);
            shoParam(general.generalId);
        }

        // 初回画面描画後の処理
        private void showPedigreeChart(int generalId)
        {

            List<General> pedGeneralList = PedigreeDao.getPedigreeChart(generalId);

            ped1bTextBlock.Text = pedGeneralList[0] is not null ? pedGeneralList[0].name + "　" + getabilityString(pedGeneralList[0].abilityType) : null;
            ped1pTextBlock.Text = pedGeneralList[1] is not null ? pedGeneralList[1].name + "　" + getabilityString(pedGeneralList[1].abilityType) : null;
            ped2bbTextBlock.Text = pedGeneralList[2] is not null ? pedGeneralList[2].name + "　" + getabilityString(pedGeneralList[2].abilityType) : null;
            ped2bpTextBlock.Text = pedGeneralList[3] is not null ? pedGeneralList[3].name + "　" + getabilityString(pedGeneralList[3].abilityType) : null;
            ped2pbTextBlock.Text = pedGeneralList[4] is not null ? pedGeneralList[4].name + "　" + getabilityString(pedGeneralList[4].abilityType) : null;
            ped2ppTextBlock.Text = pedGeneralList[5] is not null ? pedGeneralList[5].name + "　" + getabilityString(pedGeneralList[5].abilityType) : null;
            ped3bbbTextBlock.Text = pedGeneralList[6] is not null ? pedGeneralList[6].name + "　" + getabilityString(pedGeneralList[6].abilityType) : null;
            ped3bbpTextBlock.Text = pedGeneralList[7] is not null ? pedGeneralList[7].name + "　" + getabilityString(pedGeneralList[7].abilityType) : null;
            ped3bpbTextBlock.Text = pedGeneralList[8] is not null ? pedGeneralList[8].name + "　" + getabilityString(pedGeneralList[8].abilityType) : null;
            ped3bppTextBlock.Text = pedGeneralList[9] is not null ? pedGeneralList[9].name + "　" + getabilityString(pedGeneralList[9].abilityType) : null;
            ped3pbbTextBlock.Text = pedGeneralList[10] is not null ? pedGeneralList[10].name + "　" + getabilityString(pedGeneralList[10].abilityType) : null;
            ped3pbpTextBlock.Text = pedGeneralList[11] is not null ? pedGeneralList[11].name + "　" + getabilityString(pedGeneralList[11].abilityType) : null;
            ped3ppbTextBlock.Text = pedGeneralList[12] is not null ? pedGeneralList[12].name + "　" + getabilityString(pedGeneralList[12].abilityType) : null;
            ped3pppTextBlock.Text = pedGeneralList[13] is not null ? pedGeneralList[13].name + "　" + getabilityString(pedGeneralList[13].abilityType) : null;


        }

        private void shoParam(int generalId)
        {
            // generalIdからGeneralを取得
            General general = GeneralDao.getGeneralByGeneralId(generalId);

            // パラメータ表示
            GeneralNameLabel.Content = general.name;
            // 攻撃
            AttackNumTextBlock.Text = general.attackAbility.ToString();
            AttackFromNumTextBlock.Text = general.attackFromNow >= 0 ? "+" + general.attackFromNow.ToString() : general.attackFromNow.ToString();
            // 防御
            DefenseNumTextBlock.Text = general.defenseAbility.ToString();
            DefenseFromNumTextBlock.Text = general.defenseFromNow >= 0 ? "+" + general.defenseFromNow.ToString() : general.defenseFromNow.ToString();
            // 機動
            ManeuverNumTextBlock.Text = general.maneuverAbility.ToString();
            ManeuverFromNumTextBlock.Text = general.maneuverFromNow >= 0 ? "+" + general.maneuverFromNow.ToString() : general.maneuverFromNow.ToString();

        }

        private String getabilityString(int ability)
        {
            String typ = "";
            switch (ability)
            {
                case 1:
                    typ = "攻";
                    break;
                case 2:
                    typ = "防";
                    break;
                case 3:
                    typ = "機";
                    break;
                default:
                    break;
            }

            return typ;
        }
    }
}
