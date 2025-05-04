using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.tech
{
    internal class TechCostCalcLogic
    {

        // techId2～6のうち、Enableが1件ある毎に返却値が1000増加
        public static int calcCostNomTech()
        {
            int costTech = 1000;

            // techId2～6のうち、Enableが1件ある毎に返却値が500増加
            // 高すぎるので一旦やめる
            /*
            App.techBean.techEnableList.ForEach(techEnable =>
            {
                if (techEnable.techId >= 2 && techEnable.techId <= 6 && techEnable.Enable)
                {
                    costTech += 500;
                }
            });
            */
            return costTech;
        }
    }
}
