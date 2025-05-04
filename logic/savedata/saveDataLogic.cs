using ListSLG.define.constants;
using ListSLG.devtools;
using ListSLG.model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace ListSLG.logic.savedata
{
    // セーブデータ操作ロジック
    internal static class saveDataLogic
    {

        // セーブデータDB初期化（ゲーム起動時）
        public static void DBDataFileInitialization()
        {

            // セーブデータDB作成先パスおよびファイルパス取得
            string DbDirPath = fileConstants.DbDirPath;
            string CurrentDbPath = fileConstants.CurrentDbPath;
            string UserDbPath = fileConstants.UserDbPath;

            // カレントデータが既に存在する場合は再作成するので削除
            if (File.Exists(CurrentDbPath))
            {
                File.Delete(CurrentDbPath);
            }
            // ディレクトリが存在しない場合は作成
            else if (!Directory.Exists(DbDirPath))
            {
                Directory.CreateDirectory(DbDirPath);
            }


            // 初期マスタリソースのストリームを取得
            StreamResourceInfo saveDataResourceInfo = Application.GetResourceStream(new Uri("ListSLG;component/resources/db/data.db", UriKind.Relative));
            if (saveDataResourceInfo == null || saveDataResourceInfo.Stream == null)
                throw new Exception("Resource not found.");

            using (FileStream fileStream = new FileStream(CurrentDbPath, FileMode.Create, FileAccess.Write))
            {
                // カレントデータDB作成
                saveDataResourceInfo.Stream.CopyTo(fileStream);

            }

            // ユーザーデータが存在しない場合のみ、ユーザーデータDB作成
            if (!File.Exists(UserDbPath))
            {

                // ユーザーデータも同じものをコピーするが、Streamなので、同じものをもう一つ取りに行く
                StreamResourceInfo userDataResourceInfo = Application.GetResourceStream(new Uri("ListSLG;component/resources/db/data.db", UriKind.Relative));
                if (userDataResourceInfo == null || userDataResourceInfo.Stream == null)
                    throw new Exception("Resource not found.");

                using (FileStream fileStream = new FileStream(UserDbPath, FileMode.Create, FileAccess.Write))
                {
                    // ユーザーデータDB作成
                    userDataResourceInfo.Stream.CopyTo(fileStream);

                }
            }


            // ジャーナルモード変更
            using (var db = new GameDataContext())
            {
                db.ChangeJournalModeToDelete();

            }
            using (var db = new UserDataContext())
            {
                db.ChangeJournalModeToDelete();

            }

        }

        // セーブ処理（カレントデータをコピー）
        public static void saveGameData(SaveData savedata)
        {

            if (savedata == null) return;

            string destination = "data" + savedata.saveDataId;

            string sourceFilePath = fileConstants.CurrentDbPath;
            string destinationFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "savedata", destination);

            try
            {
                // ファイルをコピーする
                File.Copy(sourceFilePath, destinationFilePath, true);
                Console.WriteLine("ファイルが正常にコピーされました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ファイルのコピー中にエラーが発生しました: " + ex.Message);
                Application.Current.Shutdown();
            }

            savedata.date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            savedata.destination = destination;
            SaveDataDao.updateSaveData(savedata);


        }

        // ロード処理（カレントデータにコピー）
        public static void loadGameData(SaveData savedata)
        {

            if (savedata.destination == null) return;

            string destinationFilePath = fileConstants.CurrentDbPath;
            string sourceFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "savedata", savedata.destination);

            try
            {
                // ファイルをコピーする
                File.Copy(sourceFilePath, destinationFilePath, true);
                Console.WriteLine("ファイルが正常にコピーされました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ファイルのコピー中にエラーが発生しました: " + ex.Message);
                Application.Current.Shutdown();
            }

        }

    }

}
