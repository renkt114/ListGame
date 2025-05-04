using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.define.constants
{
    static class fileConstants
    {
        // セーブデータフォルダパス
        // 定数ではなく毎回取得しているが仕方なく。。
        public static String DbDirPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "savedata");

        // カレントセーブデータパス
        public static String CurrentDbPath = Path.Combine(DbDirPath, "gamedata");

        // ユーザーセーブデータパス
        public static String UserDbPath = Path.Combine(DbDirPath, "userdata");
    }
}
