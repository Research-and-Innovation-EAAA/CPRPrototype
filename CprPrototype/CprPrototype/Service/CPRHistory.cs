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

        public ObservableCollection<CPRHistoryEntry> Entries { get; private set; }

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

        /// <summary>
        /// Adds an item the the log of the app.
        /// </summary>
        /// <param name="name"></param>
        public void AddItem(string name,string source)
        {
            var item = new CPRHistoryEntry(name + " - Cyklus: " + ViewModel.BaseViewModel.Instance.TotalElapsedCycles, DateTime.Now,source);
            item.DateTimeString = item.Date.ToString("{0:MM/dd/yy H:mm:ss}");

            List<CPRHistoryEntry> list = new List<CPRHistoryEntry>(Entries);
            list.Add(item);

            list.Sort((x, y) => y.Date.CompareTo(x.Date));
            //list.Reverse();
                
            Entries.Clear();

            foreach (var i in list)
            {
                Entries.Add(i);
                
            }
        }

        public void AddItem(string name)
        {
            CPRHistoryEntry entry = new CPRHistoryEntry(name,DateTime.Now);
            entry.Name = name;

            Entries.Add(entry);
        }

        public void AddItems(string name, string source)
        {
            var item = new CPRHistoryEntry(name, DateTime.Now,source);
            item.DateTimeString = item.Date.ToString("{0:MM/dd/yy H:mm:ss}");

            List<CPRHistoryEntry> list = new List<CPRHistoryEntry>(Entries);
            list.Add(item);

            list.Sort((x, y) => y.Date.CompareTo(x.Date));
            //list.Reverse();
                
            Entries.Clear();

            foreach (var i in list)
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

        //
        public string ImageSource { get; set; }

        public CPRHistoryEntry(string name, DateTime date,string source)
        {
            Name = name;
            Date = date;
            ImageSource = source;
        }
        public CPRHistoryEntry(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }

        #endregion
    }
}
