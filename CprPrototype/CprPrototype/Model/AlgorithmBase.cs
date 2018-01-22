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

        private List<AlgorithmStep> steps;
        private List<Drug> drugs;
        private DateTime? startTime;
        private TimeSpan stepTime;
        private AssessmentStep step1;
        private AlgorithmStep smallShock1, smallShock2, smallNShock1, smallNShock2, exit1, currentStep;
        private StepSize stepSize;

        private int cycles = 0;

        /// <summary>
        /// Property Changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Current step in the algorithm.
        /// </summary>
        public AlgorithmStep CurrentStep
        {
            get { return currentStep; }
            set
            {
                if (currentStep != value)
                {
                    currentStep = value;

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
        /// Returns the number of cycles the algorithm went through.
        /// </summary>
        public int Cycles { get { return cycles; } }

        /// <summary>
        /// Returns the first step in the algorithm.
        /// </summary>
        public AlgorithmStep FirstStep { get { return steps[0]; } }

        /// <summary>
        /// Returns the starting Date and Time for current
        /// resuscitation process.
        /// </summary>
        public DateTime? StartTime { get { return startTime; } }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public AlgorithmBase(StepSize stepSize)
        {
            steps = InitializeSteps(stepSize);
            drugs = new DrugFactory().CreateDrugs();
            stepTime = TimeSpan.FromMinutes(2);
            this.stepSize = stepSize;
            
            // Select first step
            CurrentStep = steps[0];
        }

        /// <summary>
        /// Consturct and Initialize algorithm steps.
        /// </summary>
        public List<AlgorithmStep> InitializeSteps(StepSize stepSize)
        {
            if (steps != null)
            {
                throw new Exception("AlgorithmBase - InitiliazeSteps() : Steps already initialized.");
            }

            List<AlgorithmStep> result = new List<AlgorithmStep>();

            // Initial Step
            step1 = new AssessmentStep("Vurder Rytmen", "Vurder patientens rytme");

            // Exit Step
            exit1 = new AlgorithmStep("Circulation restored", "Continue with further resuscitation");

            switch (stepSize)
            {
                case StepSize.Small:
                    // Shockable Steps
                    smallShock1 = new AlgorithmStep("Stød en gang", "Minimer afbrydelser");
                    smallShock1.RythmStyle = RythmStyle.Shockable;
                    smallShock2 = new AlgorithmStep("HLR 2 Minutter", "Fortsæt HLR for den resterende tid");
                    smallShock2.RythmStyle = RythmStyle.Shockable;

                    // Non-Shockable Steps
                    smallNShock1 = new AlgorithmStep("Giv 1mg Adrenalin", "Med det samme");
                    smallNShock1.RythmStyle = RythmStyle.NonShockable;
                    smallNShock2 = new AlgorithmStep("HLR 2 Minutter", "Fortsæt HLR for den resterende tid");
                    smallNShock2.RythmStyle = RythmStyle.Shockable;

                    // Setup Step Relations
                    step1.PreviousStep = null;
                    step1.CircRestoredStep = exit1;

                    smallShock1.PreviousStep = step1;
                    smallShock1.NextStep = smallShock2;
                    smallShock2.PreviousStep = smallShock1;
                    smallShock2.NextStep = step1;

                    smallNShock1.PreviousStep = step1;
                    smallNShock1.NextStep = smallNShock2;
                    smallNShock2.PreviousStep = smallNShock1;
                    smallNShock2.NextStep = step1;

                    exit1.PreviousStep = step1;

                    // Add everything to the list
                    result.Add(step1);
                    result.Add(smallShock1);
                    result.Add(smallShock2);
                    result.Add(smallNShock1);
                    result.Add(smallNShock2);
                    result.Add(exit1);
                    break;
                case StepSize.Big:
                    // Shockable Steps
                    smallShock1 = new AlgorithmStep("Stød en gang", "Fortsæt HLR");
                    smallShock1.RythmStyle = RythmStyle.Shockable;

                    // Non-Shockable Steps
                    smallNShock1 = new AlgorithmStep("Giv 1mg Adrenalin", "Fortsæt HLR");
                    smallNShock1.RythmStyle = RythmStyle.NonShockable;
                    smallNShock2 = new AlgorithmStep("Fortsæt HLR ", "");
                    smallNShock2.RythmStyle = RythmStyle.NonShockable;

                    // Setup Step Relations
                    step1.PreviousStep = null;
                    step1.CircRestoredStep = exit1;

                    smallShock1.PreviousStep = step1;
                    smallShock1.NextStep = step1;

                    smallNShock1.PreviousStep = step1;
                    smallNShock1.NextStep = step1;
                    smallNShock2.PreviousStep = step1;
                    smallNShock2.NextStep = step1;

                    exit1.PreviousStep = step1;

                    // Add everything to the list
                    result.Add(step1);
                    result.Add(smallShock1);
                    result.Add(smallNShock1);
                    result.Add(smallNShock2);
                    result.Add(exit1);
                    break;
            }

            

            return result;
        }

        #endregion

        #region Timer Related Methods

        /// <summary>
        /// Adds drug shots to the drug queue if it
        /// does not exist in it already.
        /// </summary>
        public void AddDrugsToQueue(ObservableCollection<DrugShot> list, RythmStyle style)
        {
            if (drugs != null && drugs.Count > 0)
            {          
                foreach (Drug drug in drugs)
                {
                   var shot = drug.GetDrugShot(Cycles, style);

                   if (shot != null && !list.Contains(shot))
                   {
                        if (cycles == 0 && CurrentStep.NextStep.RythmStyle == RythmStyle.NonShockable)
                        {
                            shot.TimeRemaining = TimeSpan.FromMinutes(2);
                            list.Add(shot);
                        }
                        else
                        {
                            shot.ResetShot();
                            list.Add(shot);
                        }
                   }
                }
            }
        }

        /// <summary>
        /// Removes Expired Drug Reminders form queue.
        /// </summary>
        public void RemoveDrugsFromQueue(ObservableCollection<DrugShot> list)
        {
            if (drugs != null && drugs.Count > 0)
            {
                foreach (DrugShot shot in list)
                {
                    var drug = drugs.Find(x => x.DrugType == shot.Drug.DrugType);
                    
                    if (shot.Injected)
                    {
                        list.Remove(shot);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Initiates the CPR sequence based on
        /// the provided rythm style.
        /// </summary>
        public void BeginSequence(RythmStyle style)
        {
            if (startTime == null)
            {
                startTime = DateTime.Now;
            }

            if (cycles == 1)
            {
                if (stepSize == StepSize.Small)
                { 
                    smallNShock2.PreviousStep = step1;
                    smallNShock2.NextStep = step1;
                    steps.Remove(smallNShock1);
                }
                else
                {
                    smallNShock2.PreviousStep = step1;
                    smallNShock2.NextStep = step1;
                    steps.Remove(smallNShock1);
                }
            }

            switch (style)
            {
                case RythmStyle.Shockable: 
                    CurrentStep.NextStep = smallShock1;
                    break;
                case RythmStyle.NonShockable:
                    if (stepSize == StepSize.Big)
                    {
                        CurrentStep.NextStep = steps[2];
                    }
                    else
                    {
                        CurrentStep.NextStep = steps[3];
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

            // Update Cycle count
            if (next.GetType().Equals(typeof(AssessmentStep)))
            {
                cycles++;
            }

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

        /// <summary>
        /// IDisposible implementation.
        /// </summary>
        public void Dispose()
        {
            this.steps = null;
        }
    }
}