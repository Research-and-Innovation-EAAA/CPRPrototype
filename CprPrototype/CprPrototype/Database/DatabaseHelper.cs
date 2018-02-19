using CprPrototype.Helpers;
using CprPrototype.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CprPrototype.Database
{

    public class DatabaseHelper
    {

        #region Properties

        public static DatabaseHelper _databaseHelper;
        private const string _sqliteDBName = "CPRHistories.db3";
        private SQLiteAsyncConnection _dbConnection;

        /// <summary>
        /// Gets the Database Connection, and makes one if not initialized
        /// </summary>
        public SQLiteAsyncConnection DBConnection {
            get
            {
                if(_dbConnection == null)
                {
                    var path = DependencyService.Get<IFileHelper>().GetLocalFilePath(_sqliteDBName);
                    _dbConnection = new SQLiteAsyncConnection(path);
                }
                return _dbConnection;
            }
        }
        
        /// <summary>
        /// Gets the current instance of the databaseHelper, implements the singleton pattern
        /// </summary>
        public static DatabaseHelper Instance
        {
            get
            {
                if (_databaseHelper == null)
                {
                    _databaseHelper = new DatabaseHelper();
                }

                return _databaseHelper;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Creates the tables for <see cref="CPRHistory"/> & <see cref="CPRHistoryEntry"/>
        /// </summary>
        /// <returns>Returns a task completed</returns>
        public async Task CreateTablesAsync()
        {
            await DBConnection.CreateTableAsync<CPRHistory>();
            await DBConnection.CreateTableAsync<CPRHistoryEntry>();
        }

        /// <summary>
        /// Inserts a new <see cref="CPRHistory"/> into the database
        /// </summary>
        /// <param name="newEntry">the new <see cref="CPRHistory"/> to add</param>
        /// <returns></returns>
        public async Task InsertCPRHistory(CPRHistory newEntry)
        {
            await DBConnection.InsertAsync(newEntry);
        }

        /// <summary>
        /// Inserts a new <see cref="CPRHistory"/> into the database
        /// </summary>
        /// <param name="newEntry">the new <see cref="CPRHistoryEntry"/> to add</param>
        /// <returns></returns>
        public async Task InsertCPREntry(CPRHistoryEntry newEntry)
        {
            await DBConnection.InsertAsync(newEntry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="historyId"></param>
        /// <returns></returns>
        public async Task<List<CPRHistoryEntry>> GetEntriesConnectedToCPRHistory(int historyId = 0)
        {
            var query = DBConnection.Table<CPRHistoryEntry>();
            List<CPRHistoryEntry> resultList = await query.Where(i => i.CPRHistoryId == historyId).ToListAsync();
            resultList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            return resultList;
        }
        
        /// <summary>
        /// Gets a table printout 
        /// </summary>
        /// <param name="tableName">The name of the table, by default it's the class name</param>
        public void GetCPRHistoryEntryTable()
        {
            var temp = DBConnection.Table<CPRHistoryEntry>();
            // var tableCount = DBConnection.ExecuteAsync("SELECT * FROM sqlite_master WHERE type = 'table' AND name = '?'", tableName);
            // Debug.WriteLine("Result: " + tableCount);
        }

        public void GetCPRHistoryTable()
        {
            var temp = DBConnection.Table<CPRHistory>();
        }



        /// <summary>
        /// Gets all the connected <see cref="CPRHistoryEntry"/> that are connected to the specified <see cref="CPRHistory"/> instance
        /// </summary>
        /// <param name="inputHistoryID">The <see cref="CPRHistory"/> that we want to get the <see cref="CPRHistoryEntry"/>'s from </param>
        /// <returns>A list ordered by <see cref="DateTime"/> of <see cref="CPRHistoryEntry"/>'s</returns>
        public async Task<List<CPRHistoryEntry>> GetAllEntriesFromCPRHistoryAsync(int inputHistoryID = 0)
        {
            var query = DBConnection.Table<CPRHistoryEntry>();

            //await query.ToListAsync().ContinueWith((t) =>
            //{
            //    foreach (var entry in t.Result)
            //        Debug.WriteLine("CPREntry: " + entry.Name, " ID: " + entry.Id);
            //});

            List<CPRHistoryEntry> resultList = await query.Where(i => i.CPRHistoryId == inputHistoryID).ToListAsync();
            resultList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            Debug.WriteLine("Sorting tester");

            return resultList;

        }

        public async Task<int> GetCountFromTableCPRHistory()
        {
            return await DBConnection.ExecuteAsync("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name != 'android_metadata' AND name != 'sqlite_sequence'");
        }

        #endregion
    }
}
