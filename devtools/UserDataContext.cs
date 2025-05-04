using ListSLG.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;
using System.IO;
using Microsoft.Extensions.Options;
using ListSLG.define.constants;
using Microsoft.Data.Sqlite;

namespace ListSLG.devtools
{

    // セーブデータ用DBコンテキスト
    internal class UserDataContext : DataContext
    {

        public string DbPath { get; }

        public UserDataContext()
        {
            // DBファイルの保存先とDBファイル名
            DbPath = fileConstants.UserDbPath;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // データソースとしてSQLiteデータベースファイルを指定する
            options.UseSqlite($"Data Source={DbPath}");

        }

        // ジャーナルモードをdeleteに変更（起動時に実行想定）
        public void ChangeJournalModeToDelete()
        {
            // DbContextのDatabaseプロパティを使用して、DatabaseFacadeオブジェクトを取得します。
            var databaseFacade = this.Database;

            // DatabaseFacadeオブジェクトからDbConnectionオブジェクトを取得します。
            var dbConnection = databaseFacade.GetDbConnection();

            // DbConnectionオブジェクトをSqliteConnectionにキャストします。
            var sqliteConnection = (SqliteConnection)dbConnection;

            // PRAGMA文を実行するコマンドを作成します。
            using (var command = sqliteConnection.CreateCommand())
            {
                // PRAGMA文を設定します。
                command.CommandText = "PRAGMA journal_mode = 'delete';";

                // 接続を開きます。
                dbConnection.Open();

                // PRAGMA文を実行します。
                command.ExecuteNonQuery();

                // 接続を閉じます。
                dbConnection.Close();
            }
        }

    }

}
