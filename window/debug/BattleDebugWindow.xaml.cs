using ListSLG.bean;
using ListSLG.logic.battle;
using ListSLG.logic.battle.potision;
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
    /// BattleDebugWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class BattleDebugWindow : Window
    {

        CorpTroopBean? alCorpTroopBean;

        CorpTroopBean? enCorpTroopBean;


        // 兵数
        int enCISol = 0;
        int enRISol = 0;
        int enLISol = 0;
        int enRCSol = 0;
        int enLCSol = 0;
        int enRRSol = 0;
        int enLRSol = 0;

        // ブロック数
        int enCISolBlock = 0;
        int enRISolBlock = 0;
        int enLISolBlock = 0;
        int enRCSolBlock = 0;
        int enLCSolBlock = 0;
        int enRRSolBlock = 0;
        int enLRSolBlock = 0;

        // 防御数
        int enCISolDef = 0;
        int enRISolDef = 0;
        int enLISolDef = 0;
        int enRCSolDef = 0;
        int enLCSolDef = 0;
        int enRRSolDef = 0;
        int enLRSolDef = 0;



        public BattleDebugWindow()
        {
            InitializeComponent();
        }


        private void CoopSet()
        {

            // 味方想定alCorpTroopBean
            this.alCorpTroopBean = new CorpTroopBean();
            Corp alCorp = new Corp();
            List<Troop> alTroopList = new List<Troop>();

            General alCIgeneral = new General();

            alCIgeneral.attackAbility = int.Parse(AlCIAtcNum.Text);
            alCIgeneral.defenseAbility = int.Parse(AlCIDefNum.Text);
            alCIgeneral.maneuverAbility = int.Parse(AlCIManNum.Text);

            Troop alCITroop = new Troop();
            alCITroop.general = alCIgeneral;
            alCITroop.maxSoldierNum = int.Parse(AlCISolNum.Text);
            alCITroop.soldierNum = int.Parse(AlCISolNum.Text);
            alCITroop.potisioinId = 1;

            alTroopList.Add(alCITroop);

            General alRIgeneral = new General();

            alRIgeneral.attackAbility = int.Parse(AlRIAtcNum.Text);
            alRIgeneral.defenseAbility = int.Parse(AlRIDefNum.Text);
            alRIgeneral.maneuverAbility = int.Parse(AlRIManNum.Text);

            Troop alRITroop = new Troop();
            alRITroop.general = alRIgeneral;
            alRITroop.maxSoldierNum = int.Parse(AlRISolNum.Text);
            alRITroop.soldierNum = int.Parse(AlRISolNum.Text);
            alRITroop.potisioinId = 2;

            alTroopList.Add(alRITroop);

            General alLIgeneral = new General();

            alLIgeneral.attackAbility = int.Parse(AlLIAtcNum.Text);
            alLIgeneral.defenseAbility = int.Parse(AlLIDefNum.Text);
            alLIgeneral.maneuverAbility = int.Parse(AlLIManNum.Text);

            Troop alLITroop = new Troop();
            alLITroop.general = alLIgeneral;
            alLITroop.maxSoldierNum = int.Parse(AlLISolNum.Text);
            alLITroop.soldierNum = int.Parse(AlLISolNum.Text);
            alLITroop.potisioinId = 3;

            alTroopList.Add(alLITroop);

            General alRCgeneral = new General();

            alRCgeneral.attackAbility = int.Parse(AlRCAtcNum.Text);
            alRCgeneral.defenseAbility = int.Parse(AlRCDefNum.Text);
            alRCgeneral.maneuverAbility = int.Parse(AlRCManNum.Text);

            Troop alRCTroop = new Troop();
            alRCTroop.general = alRCgeneral;
            alRCTroop.maxSoldierNum = int.Parse(AlRCSolNum.Text);
            alRCTroop.soldierNum = int.Parse(AlRCSolNum.Text);
            alRCTroop.potisioinId = 4;

            alTroopList.Add(alRCTroop);

            General alLCgeneral = new General();

            alLCgeneral.attackAbility = int.Parse(AlLCAtcNum.Text);
            alLCgeneral.defenseAbility = int.Parse(AlLCDefNum.Text);
            alLCgeneral.maneuverAbility = int.Parse(AlLCManNum.Text);
            
            Troop alLCTroop = new Troop();
            alLCTroop.general = alLCgeneral;
            alLCTroop.maxSoldierNum = int.Parse(AlLCSolNum.Text);
            alLCTroop.soldierNum = int.Parse(AlLCSolNum.Text);
            alLCTroop.potisioinId = 5;

            alTroopList.Add(alLCTroop);

            General alRRgeneral = new General();
            
            alRRgeneral.attackAbility = int.Parse(AlRRAtcNum.Text);
            alRRgeneral.defenseAbility = int.Parse(AlRRDefNum.Text);
            alRRgeneral.maneuverAbility = int.Parse(AlRRManNum.Text);

            Troop alRRTroop = new Troop();
            alRRTroop.general = alRRgeneral;
            alRRTroop.maxSoldierNum = int.Parse(AlRRSolNum.Text);
            alRRTroop.soldierNum = int.Parse(AlRRSolNum.Text);
            alRRTroop.potisioinId = 6;

            alTroopList.Add(alRRTroop);

            General alLRgeneral = new General();

            alLRgeneral.attackAbility = int.Parse(AlLRAtcNum.Text);
            alLRgeneral.defenseAbility = int.Parse(AlLRDefNum.Text);
            alLRgeneral.maneuverAbility = int.Parse(AlLRManNum.Text);

            Troop alLRTroop = new Troop();
            alLRTroop.general = alLRgeneral;
            alLRTroop.maxSoldierNum = int.Parse(AlLRSolNum.Text);
            alLRTroop.soldierNum = int.Parse(AlLRSolNum.Text);
            alLRTroop.potisioinId = 7;

            alTroopList.Add(alLRTroop);

            alCorp.troop = alTroopList;

            alCorpTroopBean.initCorpTroopData(alCorp);


            // 敵想定alCorpTroopBean
            this.enCorpTroopBean = new CorpTroopBean();
            Corp enCorp = new Corp();
            List<Troop> enTroopList = new List<Troop>();

            General enCIgeneral = new General();

            enCIgeneral.attackAbility = int.Parse(EnCIAtcNum.Text);
            enCIgeneral.defenseAbility = int.Parse(EnCIDefNum.Text);
            enCIgeneral.maneuverAbility = int.Parse(EnCIManNum.Text);

            Troop enCITroop = new Troop();
            enCITroop.general = enCIgeneral;
            enCITroop.maxSoldierNum = int.Parse(EnCISolNum.Text);
            enCITroop.soldierNum = int.Parse(EnCISolNum.Text);
            enCITroop.potisioinId = 1;

            enTroopList.Add(enCITroop);

            General enRIgeneral = new General();

            enRIgeneral.attackAbility = int.Parse(EnRIAtcNum.Text);
            enRIgeneral.defenseAbility = int.Parse(EnRIDefNum.Text);
            enRIgeneral.maneuverAbility = int.Parse(EnRIManNum.Text);

            Troop enRITroop = new Troop();
            enRITroop.general = enRIgeneral;
            enRITroop.maxSoldierNum = int.Parse(EnRISolNum.Text);
            enRITroop.soldierNum = int.Parse(EnRISolNum.Text);
            enRITroop.potisioinId = 2;

            enTroopList.Add(enRITroop);

            General enLIgeneral = new General();

            enLIgeneral.attackAbility = int.Parse(EnLIAtcNum.Text);
            enLIgeneral.defenseAbility = int.Parse(EnLIDefNum.Text);
            enLIgeneral.maneuverAbility = int.Parse(EnLIManNum.Text);

            Troop enLITroop = new Troop();
            enLITroop.general = enLIgeneral;
            enLITroop.maxSoldierNum = int.Parse(EnLISolNum.Text);
            enLITroop.soldierNum = int.Parse(EnLISolNum.Text);
            enLITroop.potisioinId = 3;

            enTroopList.Add(enLITroop);

            General enRCgeneral = new General();

            enRCgeneral.attackAbility = int.Parse(EnRCAtcNum.Text);
            enRCgeneral.defenseAbility = int.Parse(EnRCDefNum.Text);
            enRCgeneral.maneuverAbility = int.Parse(EnRCManNum.Text);

            Troop enRCTroop = new Troop();
            enRCTroop.general = enRCgeneral;
            enRCTroop.maxSoldierNum = int.Parse(EnRCSolNum.Text);
            enRCTroop.soldierNum = int.Parse(EnRCSolNum.Text);
            enRCTroop.potisioinId = 4;

            enTroopList.Add(enRCTroop);

            General enLCgeneral = new General();

            enLCgeneral.attackAbility = int.Parse(EnLCAtcNum.Text);
            enLCgeneral.defenseAbility = int.Parse(EnLCDefNum.Text);
            enLCgeneral.maneuverAbility = int.Parse(EnLCManNum.Text);

            Troop enLCTroop = new Troop();
            enLCTroop.general = enLCgeneral;
            enLCTroop.maxSoldierNum = int.Parse(EnLCSolNum.Text);
            enLCTroop.soldierNum = int.Parse(EnLCSolNum.Text);
            enLCTroop.potisioinId = 5;

            enTroopList.Add(enLCTroop);

            General enRRgeneral = new General();

            enRRgeneral.attackAbility = int.Parse(EnRRAtcNum.Text);
            enRRgeneral.defenseAbility = int.Parse(EnRRDefNum.Text);
            enRRgeneral.maneuverAbility = int.Parse(EnRRManNum.Text);

            Troop enRRTroop = new Troop();
            enRRTroop.general = enRRgeneral;
            enRRTroop.maxSoldierNum = int.Parse(EnRRSolNum.Text);
            enRRTroop.soldierNum = int.Parse(EnRRSolNum.Text);
            enRRTroop.potisioinId = 6;

            enTroopList.Add(enRRTroop);

            General enLRgeneral = new General();

            enLRgeneral.attackAbility = int.Parse(EnLRAtcNum.Text);
            enLRgeneral.defenseAbility = int.Parse(EnLRDefNum.Text);
            enLRgeneral.maneuverAbility = int.Parse(EnLRManNum.Text);

            Troop enLRTroop = new Troop();
            enLRTroop.general = enLRgeneral;
            enLRTroop.maxSoldierNum = int.Parse(EnLRSolNum.Text);
            enLRTroop.soldierNum = int.Parse(EnLRSolNum.Text);
            enLRTroop.potisioinId = 7;

            enTroopList.Add(enLRTroop);


            enCorp.troop = enTroopList;

            enCorpTroopBean.initCorpTroopData(enCorp);



        }

        private void numReset()
        {

            // 兵数
            enCISol = 0;
            enRISol = 0;
            enLISol = 0;
            enRCSol = 0;
            enLCSol = 0;
            enRRSol = 0;
            enLRSol = 0;

            // ブロック数
            enCISolBlock = 0;
            enRISolBlock = 0;
            enLISolBlock = 0;
            enRCSolBlock = 0;
            enLCSolBlock = 0;
            enRRSolBlock = 0;
            enLRSolBlock = 0;

            // 防御数
            enCISolDef = 0;
            enRISolDef = 0;
            enLISolDef = 0;
            enRCSolDef = 0;
            enLCSolDef = 0;
            enRRSolDef = 0;
            enLRSolDef = 0;

            // ブロック数
            EnCISolBlockNum.Content = 0;
            EnRISolBlockNum.Content = 0;
            EnLISolBlockNum.Content = 0;
            EnRCSolBlockNum.Content = 0;
            EnLCSolBlockNum.Content = 0;
            EnRRSolBlockNum.Content = 0;
            EnLRSolBlockNum.Content = 0;

            // 防御数
            EnCISolDefNum.Content = 0;
            EnRISolDefNum.Content = 0;
            EnLISolDefNum.Content = 0;
            EnRCSolDefNum.Content = 0;
            EnLCSolDefNum.Content = 0;
            EnRRSolDefNum.Content = 0;
            EnLRSolDefNum.Content = 0;

            EnCISolADamegeNum.Content = 0;
            EnRISolADamegeNum.Content = 0;
            EnLISolADamegeNum.Content = 0;
            EnRCSolADamegeNum.Content = 0;
            EnLCSolADamegeNum.Content = 0;
            EnRRSolADamegeNum.Content = 0;
            EnLRSolADamegeNum.Content = 0;



        }

        private void battleDebug(Troop troop, PotisionBattleLogicBase battleLogicBase)
        {

            // 戦闘結果
            BattleLogicResponse battleLogicResponse = new BattleLogicResponse();

            int loopCount = int.Parse(BattleCountNum.Text);


            //10回ループ
            for (int i = 0; i < loopCount; i++)
            {
                // 戦闘処理
                battleLogicResponse = battleLogicBase.attack(alCorpTroopBean, enCorpTroopBean, CorpIdentDiv.allyCorp);

                // 戦闘結果を元に兵数を更新
                enCISol += (int.Parse(EnCISolNum.Text) - battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(1).soldierNum);
                enRISol += (int.Parse(EnRISolNum.Text) - battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(2).soldierNum);
                enLISol += (int.Parse(EnLISolNum.Text) - battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(3).soldierNum);
                enRCSol += (int.Parse(EnRCSolNum.Text) - battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(4).soldierNum);
                enLCSol += (int.Parse(EnLCSolNum.Text) - battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(5).soldierNum);
                enRRSol += (int.Parse(EnRRSolNum.Text) - battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(6).soldierNum);
                enLRSol += (int.Parse(EnLRSolNum.Text) - battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(7).soldierNum);



                // 兵数リセット
                battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(1).soldierNum = int.Parse(EnCISolNum.Text);
                battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(2).soldierNum = int.Parse(EnRISolNum.Text);
                battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(3).soldierNum = int.Parse(EnLISolNum.Text);
                battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(4).soldierNum = int.Parse(EnRCSolNum.Text);
                battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(5).soldierNum = int.Parse(EnLCSolNum.Text);
                battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(6).soldierNum = int.Parse(EnRRSolNum.Text);
                battleLogicResponse.OpponentCorpTroopBean.getTroopByPositionId(7).soldierNum = int.Parse(EnLRSolNum.Text);



            }

            enCISolBlock += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(1).blockTimes;
            enRISolBlock += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(2).blockTimes;
            enLISolBlock += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(3).blockTimes;
            enRCSolBlock += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(4).blockTimes;
            enLCSolBlock += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(5).blockTimes;
            enRRSolBlock += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(6).blockTimes;
            enLRSolBlock += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(7).blockTimes;


            enCISolDef += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(1).defTimes;
            enRISolDef += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(2).defTimes;
            enLISolDef += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(3).defTimes;
            enRCSolDef += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(4).defTimes;
            enLCSolDef += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(5).defTimes;
            enRRSolDef += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(6).defTimes;
            enLRSolDef += battleLogicResponse.OpponentCorpTroopBean.getResultByPositionId(7).defTimes;

            enCorpTroopBean.result1 = new ResultBean();
            enCorpTroopBean.result2 = new ResultBean();
            enCorpTroopBean.result3 = new ResultBean();
            enCorpTroopBean.result4 = new ResultBean();
            enCorpTroopBean.result5 = new ResultBean();
            enCorpTroopBean.result6 = new ResultBean();
            enCorpTroopBean.result7 = new ResultBean();

        }




        private void result()
        {
            // 戦闘結果を表示
            EnCISolDamegeNum.Content = enCISol;
            EnRISolDamegeNum.Content = enRISol;
            EnLISolDamegeNum.Content = enLISol;
            EnRCSolDamegeNum.Content = enRCSol;
            EnLCSolDamegeNum.Content = enLCSol;
            EnRRSolDamegeNum.Content = enRRSol;
            EnLRSolDamegeNum.Content = enLRSol;

            EnCISolBlockNum.Content = enCISolBlock;
            EnRISolBlockNum.Content = enRISolBlock;
            EnLISolBlockNum.Content = enLISolBlock;
            EnRCSolBlockNum.Content = enRCSolBlock;
            EnLCSolBlockNum.Content = enLCSolBlock;
            EnRRSolBlockNum.Content = enRRSolBlock;
            EnLRSolBlockNum.Content = enLRSolBlock;

            EnCISolDefNum.Content = enCISolDef;
            EnRISolDefNum.Content = enRISolDef;
            EnLISolDefNum.Content = enLISolDef;
            EnRCSolDefNum.Content = enRCSolDef;
            EnLCSolDefNum.Content = enLCSolDef;
            EnRRSolDefNum.Content = enRRSolDef;
            EnLRSolDefNum.Content = enLRSolDef;

            if (enCISolDef != 0)
            {
                EnCISolADamegeNum.Content = enCISol / enCISolDef;
            }
            if (enRISolDef != 0)
            {
                EnRISolADamegeNum.Content = enRISol / enRISolDef;
            }
            if (enLISolDef != 0)
            {
                EnLISolADamegeNum.Content = enLISol / enLISolDef;
            }
            if (enRCSolDef != 0)
            {
                EnRCSolADamegeNum.Content = enRCSol / enRCSolDef;
            }
            if (enLCSolDef != 0)
            {
                EnLCSolADamegeNum.Content = enLCSol / enLCSolDef;
            }
            if (enRRSolDef != 0)
            {
                EnRRSolADamegeNum.Content = enRRSol / enRRSolDef;
            }
            if (enLRSolDef != 0)
            {
                EnLRSolADamegeNum.Content = enLRSol / enLRSolDef;
            }


        }



        private void CI_Button_Click(object sender, RoutedEventArgs e)
        {

            CoopSet();
            numReset();

            Troop myTroop = alCorpTroopBean.getTroopByPositionId(1);
            CenterInfantryPotisionBattleLogic battleLogic = new CenterInfantryPotisionBattleLogic(myTroop, CorpIdentDiv.allyCorp);

            battleDebug(myTroop, battleLogic);

            result();
        }

        private void RI_Button_Click(object sender, RoutedEventArgs e)
        {

            CoopSet();
            numReset();

            Troop myTroop = alCorpTroopBean.getTroopByPositionId(2);
            RightInfantryPotisionBattleLogic battleLogic = new RightInfantryPotisionBattleLogic(myTroop, CorpIdentDiv.allyCorp);


            battleDebug(myTroop, battleLogic);

            result();

        }

        private void LI_Button_Click(object sender, RoutedEventArgs e)
        {

            CoopSet();
            numReset();

            Troop myTroop = alCorpTroopBean.getTroopByPositionId(3);
            LeftInfantryPotisionBattleLogic battleLogic = new LeftInfantryPotisionBattleLogic(myTroop, CorpIdentDiv.allyCorp);

            battleDebug(myTroop, battleLogic);

            result();


        }

        private void RC_Button_Click(object sender, RoutedEventArgs e)
        {

            CoopSet();
            numReset();

            Troop myTroop = alCorpTroopBean.getTroopByPositionId(4);
            RightCavalryPotisionBattleLogic battleLogic = new RightCavalryPotisionBattleLogic(myTroop, CorpIdentDiv.allyCorp);

            battleDebug(myTroop, battleLogic);

            result();

        }

        private void LC_Button_Click(object sender, RoutedEventArgs e)
        {

            CoopSet();
            numReset();

            Troop myTroop = alCorpTroopBean.getTroopByPositionId(5);
            LeftCavalryPotisionBattleLogic battleLogic = new LeftCavalryPotisionBattleLogic(myTroop, CorpIdentDiv.allyCorp);

            battleDebug(myTroop, battleLogic);

            result();

        }

        private void RR_Button_Click(object sender, RoutedEventArgs e)
        {

            CoopSet();
            numReset();

            Troop myTroop = alCorpTroopBean.getTroopByPositionId(6);
            RightRangedPotisionBattleLogic battleLogic = new RightRangedPotisionBattleLogic(myTroop, CorpIdentDiv.allyCorp);

            battleDebug(myTroop, battleLogic);

            result();

        }

        private void LR_Button_Click(object sender, RoutedEventArgs e)
        {

            CoopSet();
            numReset();

            Troop myTroop = alCorpTroopBean.getTroopByPositionId(7);
            LeftRangedPotisionBattleLogic battleLogic = new LeftRangedPotisionBattleLogic(myTroop, CorpIdentDiv.allyCorp);

            battleDebug(myTroop, battleLogic);

            result();

        }


        private void All_Button_Click(object sender, RoutedEventArgs e)
        {
            CoopSet();
            numReset();

            Troop myTroop1 = alCorpTroopBean.getTroopByPositionId(1);
            CenterInfantryPotisionBattleLogic battleLogic1 = new CenterInfantryPotisionBattleLogic(myTroop1, CorpIdentDiv.allyCorp);
            battleDebug(myTroop1, battleLogic1);

            Troop myTroop2 = alCorpTroopBean.getTroopByPositionId(2);
            RightInfantryPotisionBattleLogic battleLogic2 = new RightInfantryPotisionBattleLogic(myTroop2, CorpIdentDiv.allyCorp);
            battleDebug(myTroop2, battleLogic2);

            Troop myTroop3 = alCorpTroopBean.getTroopByPositionId(3);
            LeftInfantryPotisionBattleLogic battleLogic3 = new LeftInfantryPotisionBattleLogic(myTroop3, CorpIdentDiv.allyCorp);
            battleDebug(myTroop3, battleLogic3);

            Troop myTroop4 = alCorpTroopBean.getTroopByPositionId(4);
            RightCavalryPotisionBattleLogic battleLogic4 = new RightCavalryPotisionBattleLogic(myTroop4, CorpIdentDiv.allyCorp);
            battleDebug(myTroop4, battleLogic4);

            Troop myTroop5 = alCorpTroopBean.getTroopByPositionId(5);
            LeftCavalryPotisionBattleLogic battleLogic5 = new LeftCavalryPotisionBattleLogic(myTroop5, CorpIdentDiv.allyCorp);
            battleDebug(myTroop5, battleLogic5);

            Troop myTroop6 = alCorpTroopBean.getTroopByPositionId(6);
            RightRangedPotisionBattleLogic battleLogic6 = new RightRangedPotisionBattleLogic(myTroop6, CorpIdentDiv.allyCorp);
            battleDebug(myTroop6, battleLogic6);

            Troop myTroop7 = alCorpTroopBean.getTroopByPositionId(7);
            LeftRangedPotisionBattleLogic battleLogic7 = new LeftRangedPotisionBattleLogic(myTroop7, CorpIdentDiv.allyCorp);
            battleDebug(myTroop7, battleLogic7);




            result();

        }
    }
}
