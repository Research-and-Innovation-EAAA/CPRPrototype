using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CprPrototype.Service
{
    /// <summary>
    /// The CPRHistory class provides the functionality to save the
    /// process of the resuscitation algorithm onto the device.
    /// </summary>
    public class CPRHistory
    {
        #region Properties

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(40)]
        public string HistoryName { get; set; }

        public string CPRHistoryTotalCycles { set; get; }

        /// <summary>
        /// Gets and sets the date and time for the resuscitation attempt is started.
        /// </summary>
        public DateTime AttemptStarted { get; set; }

        /// <summary>
        /// Gets and sets the date and time for the resuscitation attempt is finished
        /// </summary>
        public DateTime AttemptFinished { get; set; }

        [Ignore]
        public ObservableCollection<CPRHistoryEntry> Entries { get; private set; }

        /// <summary>
        /// Gets and set the imagesource connected to the <see cref="CPRHistory"/> 
        /// </summary>
        public string ImageSource { get; set; }
        
        #endregion

        #region Contruction

        /// <summary>
        /// Initializes a new instance of the <see cref="CPRHistory"/> class
        /// </summary>
        public CPRHistory()
        {
            Entries = new ObservableCollection<CPRHistoryEntry>();
        }

        #endregion

        #region Methods

        private string Translate(string text)
        {
            return CprPrototype.Service.Translator.Instance.Translate(text);
        }

        /// <summary>
        /// Adds an item to the log of the app with an image attached.
        /// </summary>
        /// <param name="name">Name of the event</param>
        /// <param name="source">Image sourcepath</param>
        public void AddItem(string name, string source = null)
        {
            var item = new CPRHistoryEntry(name + " - " + 
            Translate("Cycle") + ": " + ViewModel.BaseViewModel.Instance.TotalElapsedCycles, DateTime.Now, source);
            item.DateTimeString = item.Date.ToString("dd/MM/yy  H:mm:ss");

            List<CPRHistoryEntry> list = new List<CPRHistoryEntry>(Entries)
            {
                item
            };

            UpdateList(list);
        }

        /// <summary>
        /// Helpermethod for ordering the incoming <see cref="CPRHistoryEntry"/>-list, 
        /// so it will be displayed correctly on screen.
        /// </summary>
        /// <param name="incomingList">List to be sorted</param>
        private void UpdateList(List<CPRHistoryEntry> incomingList)
        {
            incomingList.Sort((x, y) => y.Date.CompareTo(x.Date));
            incomingList.Reverse();
            
            Entries.Clear();

            foreach (var i in incomingList)
            {
                Entries.Add(i);

            }
        }

        #endregion
    }

    /// <summary>
    /// Entry class for our CPRHistory Records.
    /// </summary>
    public class CPRHistoryEntry
    {
        #region Properties
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(CPRHistory))]
        public int CPRHistoryId { get; set; }

        [ManyToOne]
        public CPRHistory CPRHistory { get; set; }

        /// <summary>
        /// Gets or sets the mame of the event occured
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time for when the event occured
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the formatted time string shown in GUI
        /// </summary>
        public string DateTimeString { get; set; }

        // Need some description
        public string ImageSource { get; set; }

        public CPRHistoryEntry()
        {

        }

        public CPRHistoryEntry(string name, DateTime date, string imageSource)
        {
            Name = name;
            Date = date;
            ImageSource = imageSource;
        }
        public CPRHistoryEntry(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }

        #endregion
    }
}
