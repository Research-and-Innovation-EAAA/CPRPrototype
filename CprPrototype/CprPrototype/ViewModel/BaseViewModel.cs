﻿using CprPrototype.Model;
using CprPrototype.Service;
using Plugin.Vibrate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Threading;
using Plugin.SimpleAudioPlayer;
using Plugin.SimpleAudioPlayer.Abstractions;
using System.IO;
using System.Reflection;

namespace CprPrototype.ViewModel
{
    /// <summary>
    /// The Base View Model class. This class links our 
    /// model and view together. Part of MVVM architecture.
    /// <remarks>
    /// All additional ViewModels should inherit the BaseViewModel.
    /// </remarks>
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties

        private static BaseViewModel _instance;
        private static object _padlock = new object(); // Object used to make singleton thread-safe
        private static object _timerPadlock = new object(); // Object used to make singleton thread-safe
        private bool _isInCall = false;

        private ObservableCollection<DrugShot> _notificationQueue = new ObservableCollection<DrugShot>();

        private AlgorithmStep _currentPosition;
        private TimeSpan _totalTime, _stepTime;
        private int _totalElapsedCycles;

        private const int CRITICAL_ALERT_TIME = 10;
        public bool _isDoneAvailable;
        public bool _isLogAvailable = true;
        private bool _enableDisableUI = false;
        private bool _isInCriticalTime = false;
        private List<CPRHistory> tempHistoryList = new List<CPRHistory>();

        public event PropertyChangedEventHandler PropertyChanged;

        public EventHandler<TimeEventArgs> CriticalTimeChanged;

        //public event EventHandler TimerElapsed;


        /// <summary>
        /// Gets the singleton instance of <see cref="BaseViewModel"/>.
        /// </summary>
        /// <remarks>
        /// Use this to access the BaseViewModel.
        /// Uses an object(padlock) to lock the critical region, to the effect of being thread-safe 
        /// </remarks>
        /// <returns>BaseViewModel Instance</returns>
        public static BaseViewModel Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new BaseViewModel();
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current dose queue.
        /// </summary>
        public ObservableCollection<DrugShot> NotificationQueue
        {
            get
            {
                return _notificationQueue;
            }
            set
            {
                if (_notificationQueue != value)
                {
                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("NotificationQueue");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current position in the resuscitation process
        /// </summary>
        public AlgorithmStep CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                if (_currentPosition != value)
                {
                    _currentPosition = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("CurrentPosition");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the total time spent in the resuscitation process.
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return _totalTime; }
            set
            {
                if (_totalTime != value)
                {
                    _totalTime = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("TotalTime");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of time for each step
        /// </summary>
        /// <example>
        /// The algorithm takes two minutes for each resuscitation attempt. 
        /// </example>
        public TimeSpan StepTime
        {
            get { return _stepTime; }
            set
            {
                if (_stepTime != value)
                {
                    _stepTime = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("StepTime");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or set the total number of cycles went through;
        /// </summary>
        public int TotalElapsedCycles
        {
            get { return _totalElapsedCycles; }
            set
            {
                if (_totalElapsedCycles != value)
                {
                    _totalElapsedCycles = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("TotalElapsedCycles");
                        NotifyPropertyChanged("TotalElapsedCyclesDisplayStr");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or set the total number of cycles went through;
        /// </summary>
        public string TotalElapsedCyclesDisplayStr
        {
            get { return TotalElapsedCycles + ". " + CprPrototype.Service.Translator.Instance.Translate("Cycle"); }
        }

        /// <summary>
        /// Gets the CPRHistory instance.
        /// </summary>
        public CPRHistory History { get; private set; }

        /// <summary>
        /// Gets the Timer object.
        /// </summary>
        public Timer CycleTimer { get; set; }

        /// <summary>
        /// Gets the Algorithm model.
        /// </summary>
        public AlgorithmBase AlgorithmBase { get; private set; }

        /// <summary>
        /// Enabling or disabling UI content on the homepage.
        /// </summary>
        public bool EnableDisableUI
        {
            set
            {
                _enableDisableUI = value;
                OnPropertyChanged(nameof(EnableDisableUI));
            }
            get
            {
                return _enableDisableUI;
            }
        }

        /// <summary>
        /// bool properties for handling visible and nonvisible buttons on overview. sets RUC and Doed buttons visibility to true.
        /// </summary>
        public bool IsDoneAvailable
        {
            set
            {
                _isDoneAvailable = value;
                OnPropertyChanged(nameof(IsDoneAvailable));
            }
            get
            {
                return _isDoneAvailable;
            }
        }

        /// <summary>
        /// bool properties for handling visible and nonvisible buttons on overview. sets log button to false
        /// </summary>
        public bool IsLogAvailable
        {
            set
            {
                _isLogAvailable = value;
                OnPropertyChanged(nameof(IsLogAvailable));
            }
            get
            {
                return _isLogAvailable;
            }
        }

        public bool IsInCriticalTime
        {
            set
            {
                _isInCriticalTime = value;
            }
            get
            {
                return _isInCriticalTime;
            }
        }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Empty contructor - Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        protected BaseViewModel()
        {
            History = new CPRHistory
            {
                AttemptStarted = DateTime.Now,
            };

            /*
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                CprPrototype.AppResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }*/
        }

        /// <summary>
        /// Initializes the <see cref="AlgorithmBase"/> class, and sets the <see cref="CurrentPosition"/>
        /// </summary>
        public void InitAlgorithmBase()
        {
            AlgorithmBase = new AlgorithmBase();
            CurrentPosition = AlgorithmBase.CurrentStep;
        }

        #endregion

        #region Methods & Events

        /// <summary>
        /// Updates the <see cref="StepTime"/> and checks timers for activating vibration
        /// </summary>
        private void UpdateStepTime()
        {
            if (AlgorithmBase.StepTime.TotalSeconds > 0)
            {
                AlgorithmBase.StepTime = AlgorithmBase.StepTime.Subtract(TimeSpan.FromSeconds(1));
                StepTime = AlgorithmBase.StepTime;

                if (StepTime.TotalSeconds <= CRITICAL_ALERT_TIME)
                {
                    lock (_timerPadlock)
                    {
                        if (_isInCall)
                            return;
                        _isInCall = true;
                    }
                    try
                    {
                        if (IsInCriticalTime == false)
                        {
                            IsInCriticalTime = true;
                            OnCriticalTimeChanged(IsInCriticalTime);
                        }
                    } 
                    finally
                    {
                        lock (_timerPadlock)
                        {
                            _isInCall = false;
                        }
                    }
                    CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0.25));
                    this.PlayMp3File(2);
                }
            }
        }

        private string Translate(string text)
        {
            return CprPrototype.Service.Translator.Instance.Translate(text);
        }

        /// <summary>
        /// Updates the notification timers and sorts them correctly
        /// </summary>
        private void UpdateNotificationTimers()
        {

            lock (_padlock)
            {
                if (_isInCall)
                    return;
                _isInCall = true;
            }
            try
            {
                if (NotificationQueue.Count != 0)
                {
                    for (int i = 0; i < NotificationQueue.Count; i++)
                    {
                        var shot = NotificationQueue[i];
                        // decrements the counter.
                        if (shot.TimeRemaining.TotalSeconds > 0)
                        {
                            shot.TimeRemaining = shot.TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                        }

                        // Checks if the "giv" button has been clicked.
                        if (shot.IsInjected)
                        {
                            shot.ShotAddressed();
                            History.AddItem(Translate(shot.Drug.DrugType.ToString()) + " " + Translate("HistoryGiven"), "icon_medicin.png");
                            AlgorithmBase.RemoveDrugsFromQueue(NotificationQueue);
                        }
                        else if (shot.IsIgnored) // Checks if the drug has been ignored
                        {
                            shot.ShotIgnored();
                            AlgorithmBase.RemoveDrugsFromQueue(NotificationQueue);
                        }

                        // Notify when we change from 'prep' drug to 'give' drug
                        if (shot.TimeRemaining.TotalSeconds == 120)
                        {
                            shot.BackgroundColor = Color.FromHex("#f1c40f");
                            shot.DrugDoseString = "Test";
                            CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0.25));
                            PlayMp3File(1);
                        }

                        // Notify constantly when drug timer is nearly done
                        if (shot.TimeRemaining.TotalSeconds < 16)
                        {
                            shot.BackgroundColor = Color.FromHex("#E74C3C");
                            CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0.25));
                            PlayMp3File(2);
                        }
                    }
                }
            }
            finally
            {
                lock (_padlock)
                {
                    _isInCall = false;
                }
            }
        }

        /// <summary>
        /// Gets and playes an mp3-file from Audio.
        /// </summary>
        /// <remarks>
        /// {PWM} - Remember to mark the file as "Embedded Property" in file properties
        /// </remarks>
        /// <param name="number">number indicating which beep needs to be played.</param>
        private void PlayMp3File(int number)
        {

            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream stream;

            if (number == 1)
                stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Audio.beep1.mp3");
            else
                stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Audio.beep2.mp3");

            var player = CrossSimpleAudioPlayer.Current;
            player.Load(stream);

            player.Play();
        }

        /// <summary>
        /// Advances the algorithm and updates the current step property.
        /// </summary>
        /// <param name="answer">Is the input string, which is the same as the name on the button as the user has pushed</param>
        public void AdvanceAlgorithm(string answer)
        {
            AddAlertSheetAnswerToHistory(answer);
            AlgorithmBase.AdvanceOneStep();
            CurrentPosition = AlgorithmBase.CurrentStep;

            if (CycleTimer != null)
            {
                CycleTimer.Dispose();
                CycleTimer = null;
            }
            TimerCallback timerDelegate = new TimerCallback(StaticNotifyTimerIncremented);
            CycleTimer = new Timer(timerDelegate, this, 100, 1000);
        }

        /// <summary>
        /// Private helpermethod to AdvanceAlgorithm, this adds an entry to the history list.
        /// </summary>
        /// <param name="answer">answer from DisplayAlertSheet</param>
        private void AddAlertSheetAnswerToHistory(string answer)
        {
            if (answer.Equals("AnswerShockGiven"))
            {
                History.AddItem(Translate("ShockGiven") + ", " + Translate("CprContinues"), "icon_shockable.png");
            }
            else
            {
                History.AddItem(Translate("ShockNotGiven") + ", " + Translate("CprContinues"), "icon_nonshockable.png");
            }
        }

        /// <summary>
        /// Occures when the timer ticks
        /// </summary>
        /// <remarks>
        /// {PWM} - This event is usually called each second, 
        /// since its job is to increment the total timer and current step time
        /// </remarks>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private static void StaticNotifyTimerIncremented(object sender)
        {
            BaseViewModel baseViewModel = (BaseViewModel)sender;
            baseViewModel.NotifyTimerIncremented();
        }

        private void NotifyTimerIncremented()
        {
            // Update Total Time
            TotalTime = TimeSpan.FromSeconds(DateTime.Now.Subtract(AlgorithmBase.StartTime.Value).TotalSeconds);

            // Update Step Time
            UpdateStepTime();

            // Update Cycles
            TotalElapsedCycles = AlgorithmBase.TotalElapsedCycles;

            // Update Drug Queue
            UpdateNotificationTimers();

        }

        /// <summary>
        /// Event handler for INotifyPropertyChanged.
        /// </summary>
        /// <remarks>
        /// This method is called by the Set accessor of each property.
        /// The CallerMemberName attribute that is applied to the optional propertyName
        /// parameter causes the property name of the caller to be substituted as an argument.
        /// </remarks>
        /// <param name="propertyName">Name of the property changed. Optional</param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));

                if (propertyName.Equals("CurrentStep") && AlgorithmBase.CurrentStep != null)
                {
                    CurrentPosition = AlgorithmBase.CurrentStep;
                }
            }
        }

        /// <summary>
        /// This method notifies a specific notify object to change its properties.
        /// fx buttons going from nonvisible to visible.
        /// </summary>
        protected virtual void OnPropertyChanged(string n)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler(this, new PropertyChangedEventArgs(n));
        }

        protected virtual void OnCriticalTimeChanged(bool isInCriticalTime)
        {
            if (CriticalTimeChanged != null)
            {
                CriticalTimeChanged(this, new TimeEventArgs() { IsInCriticalTime = isInCriticalTime });
            }
        }

        /// <summary>
        /// This method ends the algorithm
        /// </summary>
        public void EndAlgorithm()
        {
            History.Entries.Clear();
            CycleTimer.Dispose();
            CycleTimer = null;
            TotalTime = TimeSpan.Zero;
            TotalElapsedCycles = 0;
            StepTime = TimeSpan.Zero;
            NotificationQueue.Clear();
            AlgorithmBase = new AlgorithmBase();
            CurrentPosition = AlgorithmBase.CurrentStep;
            EnableDisableUI = false;
            IsLogAvailable = true;
            IsDoneAvailable = false;
            OnCriticalTimeChanged(false);
        }

        #endregion
    }

    /// <summary>
    /// This class enables us to send custom event arguments in OnCriticalTimeChanged eventhandler
    /// </summary>
    public class TimeEventArgs : EventArgs
    {
        public bool IsInCriticalTime { get; set; }
    }
}
