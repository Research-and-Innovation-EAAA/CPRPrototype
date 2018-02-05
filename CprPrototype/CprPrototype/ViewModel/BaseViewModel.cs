using AdvancedTimer.Forms.Plugin.Abstractions;
using CprPrototype.Model;
using CprPrototype.Service;
using Plugin.Vibrate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

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

        private static BaseViewModel instance;
        private static readonly object padlock = new object(); // Object used to make singleton thread-safe

        private AlgorithmBase algoBase;
        private CPRHistory history = new CPRHistory();

        private ObservableCollection<DrugShot> doseQueue = new ObservableCollection<DrugShot>();
        private AlgorithmStep currStep;
        private TimeSpan totalTime, stepTime;
        private int totalElapsedCycles;
        private const int CRITICAL_ALERT_TIME = 10;

        private IAdvancedTimer timer = DependencyService.Get<IAdvancedTimer>();
        private bool timerStarted = false;

        public event PropertyChangedEventHandler PropertyChanged;
        //public event EventHandler TimerElapsed;
        

        /// <summary>
        /// NextStepProperty accessor.
        /// </summary>
        public AlgorithmStep CurrentPosition
        {
            get
            {
                return currStep;
            }
            set
            {
                if (currStep != value)
                {
                    currStep = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("CurrentPosition");
                    }
                }
            }
        }

        /// <summary>
        /// Total time spent in the resuscitation process.
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set
            {
                if (totalTime != value)
                {
                    totalTime = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("TotalTime");
                    }
                }
            }
        }

        /// <summary>
        /// The amount of time for each step
        /// </summary>
        /// <example>
        /// The algorithm takes two minutes for each resuscitation attempt. 
        /// </example>
        public TimeSpan StepTime
        {
            get { return stepTime; }
            set
            {
                if (stepTime != value)
                {
                    stepTime = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("StepTime");
                    }
                }
            }
        }

        /// <summary>
        /// Returns the current dose queue.
        /// </summary>
        public ObservableCollection<DrugShot> DoseQueue
        {
            get
            {
                return doseQueue;
            }
            set
            {
                if (doseQueue != value)
                {
                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("DoseQueue");
                    }
                }
            }
        }

        /// <summary>
        /// Returns the total number of cycles we went through;
        /// </summary>
        public int TotalElapsedCycles
        {
            get { return totalElapsedCycles; }
            set
            {
                if (totalElapsedCycles != value)
                {
                    totalElapsedCycles = value;

                    if (PropertyChanged != null)
                    {
                        NotifyPropertyChanged("TotalElapsedCycles");
                    }
                }
            }
        }

        /// <summary>
        /// Returns the CPRHistory instance.
        /// </summary>
        public CPRHistory History
        {
            get { return history; }
        }

        /// <summary>
        /// The Timer object.
        /// </summary>
        public IAdvancedTimer Timer { get { return timer; } }

        /// <summary>
        /// The Algorithm Model.
        /// </summary>
        public AlgorithmBase Algorithm { get { return algoBase; } }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseViewModel()
        {
           
        }

        /// <summary>
        /// Sets up the algorithm
        /// </summary>
        public void InitAlgorithmBase()
        {
            algoBase = new AlgorithmBase();
            CurrentPosition = algoBase.CurrentStep;
        }

        /// <summary>
        /// TimerElapsed event handler.
        /// </summary>
        /// <remarks>
        /// {PWM} - This event is usually called each second, 
        /// since its job is to increment the total timer and current step time
        /// </remarks>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void NotifyTimerIncremented(object sender, EventArgs e)
        {
            // Debug.WriteLine("Entered NotifyTimerIncremented!");
            // Update Total Time
            TotalTime = TimeSpan.FromSeconds(DateTime.Now.Subtract(Algorithm.StartTime.Value).TotalSeconds);


            // Update Step Time
            // Denne skal nok fjernes, da de skal slåes sammen med AlgorithmStep
            //if (CurrentPosition.GetType().Equals(typeof(AssessmentStep)))
            if (CurrentPosition.StepType.Equals(StepType.HLRStep))
            {
                if (Algorithm.StepTime.TotalSeconds > 0)
                {
                    Algorithm.StepTime = Algorithm.StepTime.Subtract(TimeSpan.FromSeconds(1));
                    StepTime = Algorithm.StepTime; 

                    if (StepTime.TotalSeconds <= CRITICAL_ALERT_TIME)
                    {
                        // Debug.WriteLine(StepTime.TotalSeconds);
                        CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(1));
                    }
                }
            }

            if (StepTime.TotalSeconds == 0)
            {
                Debug.WriteLine(StepTime.Ticks.ToString());
                Debug.WriteLine("I've reached zero!");

            }

            

            // Update Cycles
            TotalElapsedCycles = Algorithm.TotalElapsedCycles;

            // Update Drug Queue
            ObservableCollection<DrugShot> list = new ObservableCollection<DrugShot>();
            foreach (DrugShot shot in DoseQueue)
            {
                if (TotalElapsedCycles == 0 && CurrentPosition.NextStep.RythmStyle == RythmStyle.NonShockable 
                    && shot.Drug.DrugType == DrugType.Adrenalin && shot.TimeRemaining.TotalMinutes > TimeSpan.FromMinutes(2).TotalMinutes)
                {
                    shot.TimeRemaining = TimeSpan.FromMinutes(2);
                }

                if (shot.TimeRemaining.TotalSeconds > 0)
                {
                    shot.TimeRemaining = shot.TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                }

                if (shot.IsInjected)
                {
                    shot.ShotAddressed();
                    History.AddItem(shot.DrugDoseString);
                    Algorithm.RemoveDrugsFromQueue(DoseQueue);
                }

                list.Add(shot);
            }

            DoseQueue.Clear();
            
            foreach (var item in list)
            {
                DoseQueue.Add(item);

                // Notify when we change from 'prep' drug to 'give' drug
                if (item.TimeRemaining.TotalSeconds == 120)
                {
                    CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0.25));
                }

                // Notify constantly when drug timer is nearly done
                if (item.TimeRemaining.TotalSeconds < 16)
                {
                    CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0.25));
                }
            }
        }

        /// <summary>
        /// Singleton instance.
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
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new BaseViewModel();
                    }

                    return instance;
                }
            }
        }

        #endregion

        #region Events & Handlers
        /// <summary>
        /// Advances the algorithm and updates the current step property.
        /// </summary>
        public void AdvanceAlgorithm()
        {
            if (!timerStarted)
            {
                timerStarted = true;
                Timer.initTimer(1000, NotifyTimerIncremented, true);
                Timer.startTimer();
            }

            if (CurrentPosition.Equals(Algorithm.FirstStep))
            {
                if (CurrentPosition.NextStep.RythmStyle == RythmStyle.Shockable)
                {
                    History.AddItem("Rytme vurderet - Stødbar");
                }
                else
                {
                    History.AddItem("Rytme vurderet - Ikke-Stødbar");
                }
            }
            else
            {
                History.AddItem(CurrentPosition.Name);
            }

            Algorithm.AdvanceOneStep();
            CurrentPosition = Algorithm.CurrentStep;
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
                // Debug.WriteLine("PropertyChanged - " + propertyName);

                if (propertyName.Equals("CurrentStep") && Algorithm.CurrentStep != null)
                {
                    CurrentPosition = Algorithm.CurrentStep;
                }
            }
        }
        #endregion
    }
}
