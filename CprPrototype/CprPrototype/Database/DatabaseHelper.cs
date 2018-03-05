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
    /// <summary>
    /// Provides a means for the PCL to interact with the underlying SQLite database 
    /// </summary>
    public class DatabaseHelper
    {

        #region Properties

        private static DatabaseHelper _databaseHelper;
        private const string _sqliteDBName = "CPRHistories.db3";
        private static SQLiteAsyncConnection _dbConnection = null;

        /// <summary>
        /// Gets the Database Connection, and makes one if not initialized
        /// </summary>
        public SQLiteAsyncConnection DBConnection
        {
            get
            {
                if (_dbConnection == null)
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
            try
            {
                await DBConnection.CreateTableAsync<CPRHistory>();
                await DBConnection.CreateTableAsync<CPRHistoryEntry>();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Exception Thrown in: CreateTableAsync");
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Inserts a new <see cref="CPRHistory"/> into the database
        /// </summary>
        /// <param name="newEntry">the new <see cref="CPRHistory"/> to add</param>
        /// <returns></returns>
        public async Task InsertCPRHistoryAsync(CPRHistory newEntry)
        {
            try
            {
                await DBConnection.InsertAsync(newEntry);
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Exception Thrown in: InsertCPRHistoryAsync");
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Inserts a new <see cref="CPRHistory"/> into the database
        /// </summary>
        /// <param name="newEntry">the new <see cref="CPRHistoryEntry"/> to add</param>
        /// <returns></returns>
        public async Task InsertCPREntryAsync(CPRHistoryEntry newEntry)
        {
            try
            {
                await DBConnection.InsertAsync(newEntry);
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Exception Thrown in: InsertCPREntryAsync");
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// PREREQUISITE - All th
        /// Inserts a list of <see cref="CPRHistoryEntry"/>'s into the database
        /// </summary>
        /// <param name="incommingList">List of <see cref="CPRHistoryEntry"/></param>
        /// <returns></returns>
        public async Task InsertListOfEntries(List<CPRHistoryEntry> incommingList)
        {
            try
            {
                await DBConnection.InsertAllAsync(incommingList);
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Exception Thrown in: InsertListOfEntries");
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Gets all the <see cref="CPRHistory"/> instances
        /// </summary>
        /// <param name="inputHistoryID">The <see cref="CPRHistory"/> that we want to get the <see cref="CPRHistoryEntry"/>'s from </param>
        /// <returns>A list ordered by <see cref="DateTime"/> of <see cref="CPRHistoryEntry"/>'s</returns>
        public async Task<List<CPRHistory>> GetCPRHistoriesAsync()
        {
            try
            {
                var query = DBConnection.Table<CPRHistory>();
                List<CPRHistory> resultList = await query.OrderByDescending(x => x.AttemptFinished).ToListAsync();
                return resultList;
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Exception Thrown in: GetCPRHistoriesAsync");
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets all the connected <see cref="CPRHistoryEntry"/>'s that are connected to the specified <see cref="CPRHistory"/> instance
        /// </summary>
        /// <param name="inputHistoryID">The <see cref="CPRHistory"/> that we want to get the <see cref="CPRHistoryEntry"/>'s from </param>
        /// <returns>A list ordered by <see cref="DateTime"/> of <see cref="CPRHistoryEntry"/>'s</returns>
        public async Task<List<CPRHistoryEntry>> GetEntriesConnectedToCPRHistoryAsync(int historyId = 0)
        {
            try
            {
                var query = DBConnection.Table<CPRHistoryEntry>();
                List<CPRHistoryEntry> resultList = await query.Where(i => i.CPRHistoryId == historyId).ToListAsync();
                resultList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                return resultList;
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Exception Thrown in: GetEntriesConnectedToCPRHistoryAsync");
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Updates the CPRHistory
        /// </summary>
        /// <param name="oldCPRHistory">the old CPRHistory</param>
        /// <returns></returns>
        public async Task UpdateCPRHistory(CPRHistory oldCPRHistory)
        {
            try
            {
                await DBConnection.UpdateAsync(oldCPRHistory);
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Exception Thrown in: UpdateCPRHistory");
                Debug.WriteLine(e.Message);
            }
        }

        #endregion
    }
}
