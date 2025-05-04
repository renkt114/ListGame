using ListSLG.dao;
using ListSLG.model;
using ListSLG.resources.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.logic.generate
{
    internal class AdviseGenerateLogic
    {

        public static void birthAdvice(General general)
        {

            string template = adviseTextResource.birth;
            string name = general.name;

            AdviceDao.createAdvice(1, string.Format(template, name));


        }

        public static void growthAdvice(General general)
        {

            string template = adviseTextResource.growth;
            string name = general.name;

            AdviceDao.createAdvice(2, string.Format(template, name));


        }

        public static void retireAdvice(General general)
        {

            string template = adviseTextResource.retire;
            string name = general.name;

            AdviceDao.createAdvice(2, string.Format(template, name));


        }

        public static void termAdvice()
        {

            string template = adviseTextResource.term;

            AdviceDao.createAdvice(2, template);


        }


    }
}
