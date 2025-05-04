using ListSLG.logic.resource;
using ListSLG.logic.tech;
using ListSLG.Migrations;
using ListSLG.model;
using ListSLG.resources.text;
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
    /// TechEnableSubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TechEnableSubWindow : Window
    {
        private int choseTechId;
        private int techCost = 0;

        public TechEnableSubWindow(int techId)
        {
            InitializeComponent();

            choseTechId = techId;

            var gamemaster = GameMasterDao.getGameMaster();
            int nowPrestage = gamemaster.prestige - PrestigeLogic.calcCostPrestige();
            int targetEra = 99;

            // テキストの設定
            techNameLabel.Content = ((TechDiv)techId).ToString();

            // 引数techIdからtechInfoTextBlockにtechInfoTextResourceの内容を設定
            switch (techId)
            {
                case 1:
                    techInfoTextBlock.Text = techInfoTextResource._1;
                    break;
                case 2:
                    techInfoTextBlock.Text = techInfoTextResource._2;
                    techCost = TechCostCalcLogic.calcCostNomTech();
                    break;
                case 3:
                    techInfoTextBlock.Text = techInfoTextResource._3;
                    techCost = TechCostCalcLogic.calcCostNomTech();
                    break;
                case 4:
                    techInfoTextBlock.Text = techInfoTextResource._4;
                    techCost = TechCostCalcLogic.calcCostNomTech();
                    break;
                case 5:
                    techInfoTextBlock.Text = techInfoTextResource._5;
                    techCost = TechCostCalcLogic.calcCostNomTech();
                    break;
                case 6:
                    techInfoTextBlock.Text = techInfoTextResource._6;
                    techCost = TechCostCalcLogic.calcCostNomTech();
                    break;
                case 7:
                    techInfoTextBlock.Text = techInfoTextResource._7;
                    targetEra = 0;
                    techCost = 5000;
                    break;
                case 8:
                    techInfoTextBlock.Text = techInfoTextResource._8;
                    targetEra = 1;
                    techCost = 5000;
                    break;
                case 9:
                    techInfoTextBlock.Text = techInfoTextResource._9;
                    targetEra = 2;
                    techCost = 5000;
                    break;
                case 10:
                    techInfoTextBlock.Text = techInfoTextResource._10;
                    targetEra = 3;
                    techCost = 5000;
                    break;
                case 11:
                    techInfoTextBlock.Text = techInfoTextResource._11;
                    targetEra = 4;
                    techCost = 5000;
                    break;
                case 12:
                    techInfoTextBlock.Text = techInfoTextResource._12;
                    break;
                default:
                    techInfoTextBlock.Text = "Invalid techId";
                    break;
            }

            // techIdに対応するTechEnableのEnableを取得
            bool enable = App.techBean.getTechEnableBool(techId);
            // ボタンの表示を変更

            if (!enable)
            {

                if(gamemaster.eraNum != targetEra && targetEra != 99)
                {
                    techCostLabel.Content = (EraDiv)targetEra + "のみアンロック可能です";
                    techConfirmButton.IsEnabled = false;
                }
                else if (nowPrestage >= techCost)
                {
                    techCostLabel.Content = "消費威信：" + techCost.ToString() + "。取り消せません";
                }
                else
                {
                    techCostLabel.Content = "消費威信：" + techCost.ToString() + "。不足しています";
                    techConfirmButton.IsEnabled = false;
                }

            }
            else
            {
                techCostLabel.Content = "";
                techConfirmButton.IsEnabled = false;
                techConfirmButton.Content = "採用済み";
            }

        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {


            this.Close();
        }

        private void techConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            App.techBean.updateTechEnable(choseTechId, true);
            GameMasterDao.addPrestige(-this.techCost);

            this.Close();

        }
    }
}
