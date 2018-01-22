using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CprPrototype.Model
{
    /// <summary>
    /// The DrugShot class represents a single Drug & Dose combination
    /// for a specific target.
    /// </summary>
    public class DrugShot : INotifyPropertyChanged
    {
        #region Properties

        private TimeSpan timeRemaining;
        private string timeRemainingString;

        public Drug Drug { get; set; }
        public DrugDoseTarget Target { get; set; }
        public string Dose { get; set; }
        public bool Injected { get; set; }
        public ICommand DrugCommand { get; set; }
        public Color TextColor
        {
            get
            {
                return Color.Black;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                //Success Color
                if (Injected)
                {
                    return Color.FromHex("A6CE38");
                }

                // Default Color
                if (timeRemaining.TotalSeconds > 120)
                {
                    return Color.LightGray;
                }
                // Warning Color
                else if (TimeRemaining.TotalSeconds > 15 && TimeRemaining.TotalSeconds <= 120)
                {
                    return Color.FromHex("#f1c40f");
                }
                else
                {
                    return Color.FromHex("#e74c3c");
                }
                
            }
        }

        public string DrugDoseString
        {
            get
            {
                switch (Drug.DrugType)
                {
                    case DrugType.Adrenalin:
                        if (TimeRemaining.TotalSeconds <= 120)
                        {
                            return "Giv " + DrugType.Adrenalin.ToString() + " " + Dose;
                        }
                        else
                        {
                            return "Klargør " + DrugType.Adrenalin.ToString() + " " + Dose;
                        }
                    case DrugType.Amiodaron:
                        if (TimeRemaining.TotalSeconds <= 120)
                        {
                            return "Giv " + DrugType.Amiodaron.ToString() + " " + Dose;
                        }
                        else
                        {
                            return "Klargør " + DrugType.Amiodaron.ToString() + " " + Dose;
                        }
                    case DrugType.Bikarbonat:
                        return DrugType.Bikarbonat.ToString() + " " + Dose;
                    case DrugType.Calcium:
                        return DrugType.Calcium.ToString() + " " + Dose;
                    case DrugType.Magnesium:
                        return DrugType.Magnesium.ToString() + " " + Dose;
                    default:
                        return string.Empty;
                }
            }
        }

        public TimeSpan TimeRemaining
        {
            get { return timeRemaining; }
            set
            {
                if (timeRemaining != value)
                {
                    timeRemaining = value;

                    if (PropertyChanged != null)
                    {
                        OnPropertyChanged("TimeRemaining");
                    }

                    UpdateTimeRemainingString();
                }
            }
        }

        public string TimeRemainingString
        {
            get
            {
                return timeRemainingString;
            }
            set
            {
                if (timeRemainingString != value)
                {
                    timeRemainingString = value;

                    if (PropertyChanged != null)
                    {
                        OnPropertyChanged("TimeRemainingString");
                    }
                }
            }
        }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="drug">Drug</param>
        /// <param name="target">Adult or Child</param>
        /// <param name="dose">Dose as string</param>
        public DrugShot(Drug drug, DrugDoseTarget target, string dose)
        {
            Drug = drug;
            Target = target;
            Dose = dose;
            Injected = false;
            DrugCommand = new Command(ShotAddressed);
            timeRemaining = Drug.PrepTime;
            UpdateTimeRemainingString();
        }

        /// <summary>
        /// Empty CTOR.
        /// </summary>
        public DrugShot()
        {
            timeRemaining = Drug.PrepTime;
            DrugCommand = new Command(ShotAddressed);
            Injected = false;
            UpdateTimeRemainingString();
        }

        #endregion

        public void ShotAddressed()
        {
            Injected = true;
            Drug.LastInjection = DateTime.Now;
        }

        public void ResetShot()
        {
            Injected = false;
            TimeRemaining = Drug.PrepTime;
        }

        public void UpdateTimeRemainingString()
        {
            int minutes = TimeRemaining.Minutes;
            int seconds = TimeRemaining.Seconds;

            if (minutes > 0)
            {
                TimeRemainingString = minutes + " min " + seconds + " secs";
            }
            else
            {
                TimeRemainingString = seconds + " secs";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event handler for INotifyPropertyChanged.
        /// </summary>
        /// <param name="propertyName">Name of the property changed. Optional</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                Debug.WriteLine("DrugShotPropertyChanged - " + propertyName);
            }
        }

    }
}
