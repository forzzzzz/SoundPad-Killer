using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace SaundpadKiller
{
    internal class Database
    {
        public SQLiteConnection Connection;

        public Database() 
        {
            Connection = new SQLiteConnection("Data Sourse=database.sqlite3");

            if (!File.Exists("./database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
            }


        }
    }
}
