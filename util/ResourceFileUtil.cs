using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using System.IO.Packaging;

namespace ListSLG.util
{
    // CSVファイル操作util
    internal class ResourceFileUtil
    {

        // 将軍名CSV取得処理
        public static List<string> GetGeneralNameByCsvFile()
        {
            return ReadCsvFile("component/resources/csv/generalName.csv");
        }


        // CSV読み取り処理
        private static List<string> ReadCsvFile(string resourcePath)
        {

            List<string> rtnStringList = new List<string>();

            StreamResourceInfo csvResourceInfo = Application.GetResourceStream(new Uri("ListSLG;" + resourcePath, UriKind.Relative));


            using (StreamReader reader = new StreamReader(csvResourceInfo.Stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    rtnStringList.Add(line);

                }
            }

            return rtnStringList;
        }
    }


}
