﻿using System;
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
        private ObservableCollection<CPRHistoryEntry> list = new ObservableCollection<CPRHistoryEntry>();

        public ObservableCollection<CPRHistoryEntry> Records
        {
            get { return list; }
        }

        //public void AddItem(string name, DateTime date)
        //{
        //    var item = new CPRHistoryEntry(name, date)
        //    {
        //        DateTimeString = date.ToString("{0:MM/dd/yy H:mm:ss}")
        //    };

        //    List<CPRHistoryEntry> list = new List<CPRHistoryEntry>(Records);
        //    list.Add(item);
            
        //    list.Sort((x, y) => y.Date.CompareTo(x.Date));
        //    //list.Reverse();

        //    Records.Clear();

        //    foreach (var i in list)
        //    {
        //        Records.Add(i);
        //    }
        //}

        /// <summary>
        /// Adds an item the the log of the app.
        /// </summary>
        /// <param name="name"></param>
        public void AddItem(string name,string source)
        {
            var item = new CPRHistoryEntry(name + " Cyklus: " + ViewModel.BaseViewModel.Instance.TotalElapsedCycles, DateTime.Now,source);
            item.DateTimeString = item.Date.ToString("{0:MM/dd/yy H:mm:ss}");

            List<CPRHistoryEntry> list = new List<CPRHistoryEntry>(Records);
            list.Add(item);

            list.Sort((x, y) => y.Date.CompareTo(x.Date));
            //list.Reverse();
                
            Records.Clear();

            foreach (var i in list)
            {
                Records.Add(i);
            }
        }

        public void AddItem(string name)
        {
            CPRHistoryEntry entry = new CPRHistoryEntry();
            entry.Name = name;

            Records.Add(entry);
        }

        public void AddItems(string name, string source)
        {
            var item = new CPRHistoryEntry(name, DateTime.Now,source);
            item.DateTimeString = item.Date.ToString("{0:MM/dd/yy H:mm:ss}");

            List<CPRHistoryEntry> list = new List<CPRHistoryEntry>(Records);
            list.Add(item);

            list.Sort((x, y) => y.Date.CompareTo(x.Date));
            //list.Reverse();

            Records.Clear();

            foreach (var i in list)
            {
                Records.Add(i);
            }
        }
    }

    /// <summary>
    /// Entry class for our CPRHistory Records.
    /// </summary>
    public class CPRHistoryEntry
    {
        /// <summary>
        /// Name of the event occured
        /// </summary>
        /// <example>
        /// Adrenalin
        /// </example>
        public string Name { get; set; }
        /// <summary>
        /// Time for when the event occured
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Formatted time string shown in GUI
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

        public CPRHistoryEntry()
        {

        }
    }
}
