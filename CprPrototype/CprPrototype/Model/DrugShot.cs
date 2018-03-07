using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CprPrototype.Model
{
    /// <summary>
    /// Represents a single Drug & Dose combination
    /// </summary>
    public class DrugShot : INotifyPropertyChanged
    {
        #region Properties
        
        private TimeSpan _timeRemaining;
        private string _timeRemainingString, _drugDoseString;
        private Color _backgroundColor;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the <see cref="Model.Drug"/> connected to the <see cref="DrugShot"/> class
        /// </summary>
        public Drug Drug { get; set; }

        /// <summary>
        /// Gets or sets the dose string
        /// </summary>
        public string Dose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the drug is injected
        /// </summary>
        public bool IsInjected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the drug is ignored
        /// </summary>
        public bool IsIgnored { get; set; }

        /// <summary>
        /// Gets or sets the assigned command
        /// </summary>
        public ICommand DrugInjectedCommand { get; set; }

        /// <summary>
        /// Gets or sets the assigned command
        /// </summary>
        public ICommand DrugIgnoredCommand { get; set; }

        /// <summary>
        /// Gets the textcolor af the notification text
        /// </summary>
        public Color TextColor
        {
            get
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// Gets the color of the drug notification background
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                ////Success Color
                if (IsInjected)
                    _backgroundColor = Color.FromHex("A6CE38");

                //// Default Color
                else if (_timeRemaining.TotalSeconds > 120)
                    _backgroundColor = Color.LightGray;

                // Warning Color
                else if (TimeRemaining.TotalSeconds > 15 && TimeRemaining.TotalSeconds <= 120)
                    _backgroundColor = Color.FromHex("#F1C40F");
                else
                    _backgroundColor = Color.FromHex("#E74C3C");

                return _backgroundColor;
            }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;

                    if (PropertyChanged != null)
                    {
                        OnPropertyChanged("BackgroundColor");
                    }
                }


            }
        }

        /// <summary>
        /// Gets a formatted String to the notification text
        /// </summary>
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
            set
            {
                if (_drugDoseString != value)
                {
                    _drugDoseString = value;

                    if (PropertyChanged != null)
                    {
                        OnPropertyChanged("DrugDoseString");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the time remaining for the <see cref="DrugShot"/>
        /// </summary>
        public TimeSpan TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                if (_timeRemaining != value)
                {
                    _timeRemaining = value;

                    if (PropertyChanged != null)
                    {
                        OnPropertyChanged("TimeRemaining");
                    }

                    UpdateTimeRemainingString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the formatted time string for <see cref="DrugShot"/>
        /// </summary>
        public string TimeRemainingString
        {
            get
            {
                return _timeRemainingString;
            }
            set
            {
                if (_timeRemainingString != value)
                {
                    _timeRemainingString = value;

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
        /// Initializes a new instance of the <see cref="DrugShot"/> class
        /// </summary>
        /// <param name="drug">Drug</param>
        /// <param name="dose">Dose as string</param>
        public DrugShot(Drug drug, string dose)
        {
            Drug = drug;
            Dose = dose;
            IsInjected = false;
            IsIgnored = false;
            DrugInjectedCommand = new Command(ShotAddressed);
            DrugIgnoredCommand = new Command(ShotIgnored);
            _timeRemaining = Drug.PreparationTime;
            UpdateTimeRemainingString();
        }

        #endregion

        #region Methods & Events
        
        /// <summary>
        /// Administers the shot and updates <see cref="Drug.TimeOfLatestInjection"/>
        /// </summary>
        public void ShotAddressed()
        {
            IsInjected = true;
            Drug.TimeOfLatestInjection = DateTime.Now;
        }

        /// <summary>
        /// Sets the drugshot as ignored
        /// </summary>
        public void ShotIgnored()
        {
            IsIgnored = true;
        }

        /// <summary>
        /// Resets the shot for a new injection 
        /// </summary>
        public void ResetShot()
        {
            IsInjected = false;
            TimeRemaining = Drug.PreparationTime;
        }

        /// <summary>
        /// Updates the notification time-string using the TimeRemaining class property.
        /// </summary>
        private void UpdateTimeRemainingString()
        {
            int minutes = TimeRemaining.Minutes;
            int seconds = TimeRemaining.Seconds;

            TimeRemainingString = "";
            if (minutes > 0)
                TimeRemainingString = minutes + " min " + seconds + " sek";
            else
                TimeRemainingString = seconds + " sek";
        }

        /// <summary>
        /// Event handler for INotifyPropertyChanged.
        /// </summary>
        /// <param name="propertyName">Name of the property changed. Optional</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //Debug.WriteLine("OnPropertyChanged Ocurred with - " + propertyName);
        }

        #endregion
    }
}
