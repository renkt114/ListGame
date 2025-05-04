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
    // DB定義およびマイグレーション用DBコンテキスト
    internal class DataContext : DbContext
    {
        // マイグレーションコマンド
        // Add-Migration xxxx -Context DataContext
        // Update-Database -Context DataContext

        public DbSet<General>? General { get; set; }
        public DbSet<Corp>? Corp { get; set; }
        public DbSet<Troop>? Troop { get; set; }
        public DbSet<Mission>? Mission { get; set; }
        public DbSet<CorpPlan>? CorpPlan { get; set; }
        public DbSet<SeedingJoin>? SeedingJoin { get; set; }
        public DbSet<RenewalGeneral>? RenewalGeneral { get; set; }
        public DbSet<GameMaster>? GameMaster { get; set; }
        public DbSet<SaveData>? SaveData { get; set; }
        public DbSet<FamilyLine>? FamilyLine { get; set; }
        public DbSet<Advice>? Advice { get; set; }
        public DbSet<Pedigree>? Pedigree { get; set; }
        public DbSet<TechEnable>? TecEnable { get; set; }

        public string DbPath { get; }

        public DataContext()
        {
            // DBファイルの保存先とDBファイル名
            DbPath = "D:\\workspace\\VisualStudioProject\\ListSLG\\resources\\db\\data.db";

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // データソースとしてSQLiteデータベースファイルを指定する
            options.UseSqlite($"Data Source={DbPath}");

        }
        

    }

}
