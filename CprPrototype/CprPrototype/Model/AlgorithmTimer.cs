using AdvancedTimer.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;

namespace CprPrototype.Model
{
    public class AlgorithmTimer
    {
        #region Properties 
        
        /// <summary>
        /// Gets or sets the interval used by <see cref="AlgorithmTimer"./>
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AlgorithmTimer"/> starttime.
        /// </summary>
        public DateTime TimerStartTime { get; set; }

        /// <summary>
        /// Gets the totalTimer object.
        /// </summary>
        public IAdvancedTimer Timer { get; private set; }

        // Event for ViewModel to subscribe to
        public event EventHandler TimerElapsedEvent;

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmTimer" class./>
        /// </summary>
        public AlgorithmTimer(int interval, bool init = true)
        {
            Timer = DependencyService.Get<IAdvancedTimer>();
            Interval = interval;

            if (init) { Initialize(); }
        }

        /// <summary>
        /// Initialize totalTimer.
        /// </summary>
        public void Initialize()
        {
            Timer.initTimer(Interval, TimerElapsedEvent, true);
        }

        #endregion

        #region Methods & Events

        /// <summary>
        /// Starts the totalTimer and sets DateTime property.
        /// </summary>
        public void StartTimer()
        {
            TimerStartTime = DateTime.Now;
            Timer.startTimer();
        }

        /// <summary>
        /// Occured when TimerElapsedEvent is invoked.
        /// </summary>
        protected virtual void OnTimerElapsedEvent()
        {
            TimerElapsedEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
