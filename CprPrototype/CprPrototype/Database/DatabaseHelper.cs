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
        private SQLiteAsyncConnection _dbConnection;

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
            await DBConnection.CreateTableAsync<CPRHistory>();
            await DBConnection.CreateTableAsync<CPRHistoryEntry>();
        }

        /// <summary>
        /// Inserts a new <see cref="CPRHistory"/> into the database
        /// </summary>
        /// <param name="newEntry">the new <see cref="CPRHistory"/> to add</param>
        /// <returns></returns>
        public async Task InsertCPRHistoryAsync(CPRHistory newEntry)
        {
            await DBConnection.InsertAsync(newEntry);
        }

        /// <summary>
        /// Inserts a new <see cref="CPRHistory"/> into the database
        /// </summary>
        /// <param name="newEntry">the new <see cref="CPRHistoryEntry"/> to add</param>
        /// <returns></returns>
        public async Task InsertCPREntryAsync(CPRHistoryEntry newEntry)
        {
            await DBConnection.InsertAsync(newEntry);
        }

        /// <summary>
        /// PREREQUISITE - All th
        /// Inserts a list of <see cref="CPRHistoryEntry"/>'s into the database
        /// </summary>
        /// <param name="incommingList">List of <see cref="CPRHistoryEntry"/></param>
        /// <returns></returns>
        public async Task InsertListOfEntries(List<CPRHistoryEntry> incommingList)
        {
            await DBConnection.InsertAllAsync(incommingList);
        }

        /// <summary>
        /// Gets all the <see cref="CPRHistory"/> instances
        /// </summary>
        /// <param name="inputHistoryID">The <see cref="CPRHistory"/> that we want to get the <see cref="CPRHistoryEntry"/>'s from </param>
        /// <returns>A list ordered by <see cref="DateTime"/> of <see cref="CPRHistoryEntry"/>'s</returns>
        public async Task<List<CPRHistory>> GetCPRHistoriesAsync()
        {
            var query = DBConnection.Table<CPRHistory>();
            List<CPRHistory> resultList = await query.ToListAsync();
            resultList.Sort((x, y) => DateTime.Compare(x.AttemptFinished, y.AttemptFinished));
            return resultList;
        }

        /// <summary>
        /// Gets all the connected <see cref="CPRHistoryEntry"/>'s that are connected to the specified <see cref="CPRHistory"/> instance
        /// </summary>
        /// <param name="inputHistoryID">The <see cref="CPRHistory"/> that we want to get the <see cref="CPRHistoryEntry"/>'s from </param>
        /// <returns>A list ordered by <see cref="DateTime"/> of <see cref="CPRHistoryEntry"/>'s</returns>
        public async Task<List<CPRHistoryEntry>> GetEntriesConnectedToCPRHistoryAsync(int historyId = 0)
        {
            var query = DBConnection.Table<CPRHistoryEntry>();
            List<CPRHistoryEntry> resultList = await query.Where(i => i.CPRHistoryId == historyId).ToListAsync();
            resultList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            return resultList;
        }

        /// <summary>
        /// Updates the CPRHistory
        /// </summary>
        /// <param name="oldCPRHistory">the old CPRHistory</param>
        /// <returns></returns>
        public async Task UpdateCPRHistory(CPRHistory oldCPRHistory)
        {
            await DBConnection.UpdateAsync(oldCPRHistory);
        }

        #endregion
    }
}
