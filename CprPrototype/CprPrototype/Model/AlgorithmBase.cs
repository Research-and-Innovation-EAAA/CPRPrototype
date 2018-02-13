using AdvancedTimer.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CprPrototype.Model
{
    /// <summary>
    /// The AlgorithmBase class represents a collection of algorithm steps,
    /// resulting in the digital form of CPR algorithm.
    /// </summary>
    public class AlgorithmBase : IDisposable, INotifyPropertyChanged
    {
        #region Properties

        private List<Drug> _drugsCollection;
        private DateTime? _startTime;
        private TimeSpan _stepTime;
        private AlgorithmStep _firstStep, _shockable1, _nonShockable1, _nonShockable2, _exitStep, _currentStep;

        private int totalElapsedCycles = 0;

        /// <summary>
        /// Property Changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Current step in the algorithm.
        /// </summary>
        public AlgorithmStep CurrentStep
        {
            get { return _currentStep; }
            set
            {
                if (_currentStep != value)
                {
                    _currentStep = value;

                    if (PropertyChanged != null)
                    {
                        OnPropertyChanged("CurrentStep");
                    }
                }
            }
        }

        /// <summary>
        /// Returns the remaining step time.
        /// </summary>
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
                        OnPropertyChanged("StepTime");
                    }
                }
            }
        }

        /// <summary>
        /// Returns the number of cycles the algorithm went through.
        /// </summary>
        public int TotalElapsedCycles { get { return totalElapsedCycles; } }

        /// <summary>
        /// Returns the first step in the algorithm.
        /// </summary>
        public AlgorithmStep FirstStep { get { return _firstStep; } }

        /// <summary>
        /// Returns the starting Date and Time for current
        /// resuscitation process.
        /// </summary>
        public DateTime? StartTime { get { return _startTime; } }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public AlgorithmBase()
        {
            InitializeAlgorithmSteps();
            _drugsCollection = new DrugFactory().CreateDrugs();
            StepTime = TimeSpan.FromMinutes(2);

            // Initiate so we start at firstStep
            CurrentStep = _firstStep;
        }

        /// <summary>
        /// Construct and Initialize algorithm steps.
        /// </summary>
        public void InitializeAlgorithmSteps()
        {
            //========================================================================
            // Initial Step
            //========================================================================

            _firstStep = new AlgorithmStep("Vurder Rytmen", "Vurder patientens rytme");

            //========================================================================
            // Shockable Steps
            //========================================================================

            _shockable1 = new AlgorithmStep("", "Fortsæt HLR")
            {
                RythmStyle = RythmStyle.Shockable,
            };

            //========================================================================
            // Non-Shockable Steps
            //========================================================================

            _nonShockable1 = new AlgorithmStep("Giv 1mg Adrenalin", "Fortsæt HLR")
            {
                RythmStyle = RythmStyle.NonShockable
            };

            _nonShockable2 = new AlgorithmStep("", "Fortsæt HLR")
            {
                RythmStyle = RythmStyle.NonShockable
            };

            //========================================================================
            // Exit Step (Currently not used)
            //========================================================================

            _exitStep = new AlgorithmStep("Circulation restored", "Continue with further resuscitation");

            //========================================================================
            // Setup Step Relations (Linked List)
            //========================================================================

            _firstStep.PreviousStep = null;

            _shockable1.PreviousStep = _firstStep;
            _shockable1.NextStep = _firstStep;

            _nonShockable1.PreviousStep = _firstStep;
            _nonShockable1.NextStep = _firstStep;

            _nonShockable2.PreviousStep = _firstStep;
            _nonShockable2.NextStep = _firstStep;

            _exitStep.PreviousStep = _firstStep;
        }

        #endregion

        #region Methods & Events

        /// <summary>
        /// Adds drug shots to the drug queue if it
        /// does not exist in it already.
        /// </summary>
        public void AddDrugsToQueue(ObservableCollection<DrugShot> notificationQueue, RythmStyle rythmStyle)
        {
            if (_drugsCollection != null && _drugsCollection.Count > 0)
            {
                foreach (Drug drug in _drugsCollection)
                {
                    var shot = drug.GetDrugShot(TotalElapsedCycles, rythmStyle);

                    if (shot != null && !notificationQueue.Contains(shot))
                    {
                        if (TotalElapsedCycles == 0 && CurrentStep.NextStep.RythmStyle == RythmStyle.NonShockable) // Handles the first Non-Shockable drug shot
                        {
                            shot.TimeRemaining = TimeSpan.FromMinutes(0);
                            notificationQueue.Add(shot);
                        }
                        else
                        {
                            shot.ResetShot();
                            notificationQueue.Add(shot);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes Expired Drug Reminders form queue.
        /// </summary>
        public void RemoveDrugsFromQueue(ObservableCollection<DrugShot> notificationQueue)
        {
            if (_drugsCollection != null && _drugsCollection.Count > 0)
            {
                foreach (DrugShot shot in notificationQueue)
                {
                    var drug = _drugsCollection.Find(x => x.DrugType == shot.Drug.DrugType);

                    if (shot.IsInjected || shot.IsIgnored)
                    {
                        notificationQueue.Remove(shot);
                    }
                }
            }
        }

        /// <summary>
        /// Initiates the CPR sequence based on
        /// the provided rythm style.
        /// </summary>
        public void BeginSequence(RythmStyle rythmStyle)
        {
            if (StartTime == null) { _startTime = DateTime.Now; }

            switch (rythmStyle)
            {
                case RythmStyle.Shockable:
                    CurrentStep.NextStep = _shockable1;
                    break;
                case RythmStyle.NonShockable:
                    if (TotalElapsedCycles != 0)
                    {
                        CurrentStep.NextStep = _nonShockable2;
                    }
                    else
                    {
                        CurrentStep.NextStep = _nonShockable1;
                    }
                    break;
            }
        }

        /// <summary>
        /// Advances to the next step.
        /// </summary>
        public void AdvanceOneStep()
        {
            var next = CurrentStep.NextStep;
            totalElapsedCycles++;


            // Sanity check
            if (next != null)
            {
                CurrentStep = next;
            }
            else
            {
                throw new OperationCanceledException("AlgorithmBase : AdvanceOneStep() - Next step is null.");
            }
        }

        /// <summary>
        /// IDisposible implementation.
        /// </summary>
        public void Dispose()
        {
            this._drugsCollection = null;
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
                Debug.WriteLine("AlgorithmPropertyChanged - " + propertyName);
            }
        }

        #endregion
    }
}