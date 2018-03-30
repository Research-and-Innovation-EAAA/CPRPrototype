using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using CprPrototype;

namespace CprPrototype.Droid.Data
{
    public class CPRDatabaseHelper
    {

        private static CPRDatabaseHelper _instance;
        private static readonly object _padlock = new object(); // Object used to make singleton thread-safe

        
        private static string databaseFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private static string pathToDatabase = System.IO.Path.Combine(databaseFolder, "db_CPRData.db");

        public static CPRDatabaseHelper Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new CPRDatabaseHelper();
                    }

                    return _instance;
                }
            }
        }


        public string CreateDatabase()
        {
            try
            {
                var connection = new SQLiteConnection(pathToDatabase);
                var temp = connection.CreateTable<CprPrototype.Service.CPRHistoryEntry>();
                Console.WriteLine(temp);
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public string InsertUpdateData(CprPrototype.Service.CPRHistoryEntry data)
        {
            try
            {
                var db = new SQLiteConnection(pathToDatabase);
                if (db.Insert(data) != 0)
                    db.Update(data);
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public int FindNumberRecords()
        {
            try
            {
                var db = new SQLiteConnection(pathToDatabase);
                // this counts all records in the database, it can be slow depending on the size of the database
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM CPRHistoryEntry");

                // for a non-parameterless query
                // var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

                return count;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }

    }
}