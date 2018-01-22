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
        private ObservableCollection<CPRHistoryEntry> list = new ObservableCollection<CPRHistoryEntry>();

        public ObservableCollection<CPRHistoryEntry> Records
        {
            get { return list; }
        }

        public void AddItem(string name, DateTime date)
        {
            var item = new CPRHistoryEntry(name, date);
            item.DateTimeString = date.ToString("{0:MM/dd/yy H:mm:ss}");

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
            var item = new CPRHistoryEntry(name, DateTime.Now);
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
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string DateTimeString { get; set; }

        public CPRHistoryEntry(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }

        public CPRHistoryEntry()
        {

        }
    }
}
