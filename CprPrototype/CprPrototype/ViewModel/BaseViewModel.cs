using AdvancedTimer.Forms.Plugin.Abstractions;
using CprPrototype.Model;
using CprPrototype.Service;
using Plugin.Vibrate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private AlgorithmBase algoBase;
        private CPRHistory history = new CPRHistory();

        private ObservableCollection<DrugShot> doseQueue = new ObservableCollection<DrugShot>();
        private AlgorithmStep currStep;
        private TimeSpan totalTime, stepTime;
        private int cycles;

        private IAdvancedTimer timer = DependencyService.Get<IAdvancedTimer>();
        private bool timerStarted = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler TimerElapsed;
        

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
                        OnPropertyChanged("CurrentPosition");
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
                        OnPropertyChanged("TotalTime");
                    }
                }
            }
        }

        /// <summary>
        /// Total time spent in the resuscitation process.
        /// </summary>
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
                        OnPropertyChanged("StepTime");
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
                        OnPropertyChanged("DoseQueue");
                    }
                }
            }
        }

        /// <summary>
        /// Returns the total number of cycles we went through;
        /// </summary>
        public int Cycles
        {
            get { return cycles; }
            set
            {
                if (cycles != value)
                {
                    cycles = value;

                    if (PropertyChanged != null)
                    {
                        OnPropertyChanged("Cycles");
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

        public StepSize StepSize { get; set; }

        /// <summary>
        /// The Timer object.
        /// </summary>
        public IAdvancedTimer Timer { get { return timer; } }

        /// <summary>
        /// The Algorithm Model.
        /// </summary>
        public AlgorithmBase Algorithm { get { return algoBase; } }


        public InteractionMode Mode { get; set; }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        protected BaseViewModel()
        {
           
        }

        public void InitAlgorithmBase(StepSize size)
        {
            StepSize = size;
            algoBase = new AlgorithmBase(size);
            CurrentPosition = algoBase.CurrentStep;
        }

        /// <summary>
        /// TimerElapsed event handler.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnTimerElapsed(object sender, EventArgs e)
        {
            // Update Total Time
            TotalTime = TimeSpan.FromSeconds(DateTime.Now.Subtract(Algorithm.StartTime.Value).TotalSeconds);

            // Update Step Time
            if (CurrentPosition.GetType().Equals(typeof(AssessmentStep)))
            {
                Algorithm.StepTime = TimeSpan.FromMinutes(2);
                StepTime = Algorithm.StepTime;
            }
            else if (CurrentPosition.Name == "HLR 2 Minutter" || CurrentPosition.Name == "Fortsæt HLR"
                || CurrentPosition.Description == "Fortsæt HLR")
            {
                if (Algorithm.StepTime.TotalSeconds > 0)
                {
                    Algorithm.StepTime = Algorithm.StepTime.Subtract(TimeSpan.FromSeconds(1));
                    StepTime = Algorithm.StepTime; 

                    if (StepTime.TotalSeconds < 21)
                    {
                        CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0.25));

                        if (Mode == InteractionMode.Sound)
                        {
                            // Play Sound
                            DependencyService.Get<IAudio>().PlayMp3File(2);
                        }
                    }
                }
            }

            // Update Cycles
            Cycles = Algorithm.Cycles;

            // Update Drug Queue
            ObservableCollection<DrugShot> list = new ObservableCollection<DrugShot>();
            foreach (DrugShot shot in DoseQueue)
            {
                if (Cycles == 0 && CurrentPosition.NextStep.RythmStyle == RythmStyle.NonShockable 
                    && shot.Drug.DrugType == DrugType.Adrenalin && shot.TimeRemaining.TotalMinutes > TimeSpan.FromMinutes(2).TotalMinutes)
                {
                    shot.TimeRemaining = TimeSpan.FromMinutes(2);
                }

                if (shot.TimeRemaining.TotalSeconds > 0)
                {
                    shot.TimeRemaining = shot.TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                }

                if (shot.Injected)
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

                    if (Mode == InteractionMode.Sound)
                    {
                        // Play Sound
                        DependencyService.Get<IAudio>().PlayMp3File(1);
                    }
                }

                // Notify constantly when drug timer is nearly done
                if (item.TimeRemaining.TotalSeconds < 16)
                {
                    CrossVibrate.Current.Vibration(TimeSpan.FromSeconds(0.25));

                    if (Mode == InteractionMode.Sound)
                    {
                        // Play Sound
                        DependencyService.Get<IAudio>().PlayMp3File(2);
                    }
                }
            }
        }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        /// <remarks>
        /// Use this to access the BaseViewModel.
        /// </remarks>
        /// <returns>BaseViewModel Instance</returns>
        public static BaseViewModel Instance()
        {
            // uses lazy initialization.
            // this is not thread safe.
            if (instance == null)
            {
                instance = new BaseViewModel();
            }

            return instance;
        }

        #endregion

        /// <summary>
        /// Advances the algorithm and updates the current step property.
        /// </summary>
        public void AdvanceAlgorithm()
        {
            if (!timerStarted)
            {
                timerStarted = true;
                Timer.initTimer(1000, OnTimerElapsed, true);
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
        /// <param name="propertyName">Name of the property changed. Optional</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                //Debug.WriteLine("PropertyChanged - " + propertyName);

                if (propertyName.Equals("CurrentStep") && Algorithm.CurrentStep != null)
                {
                    CurrentPosition = Algorithm.CurrentStep;
                }
            }
        }
        
    }
}
