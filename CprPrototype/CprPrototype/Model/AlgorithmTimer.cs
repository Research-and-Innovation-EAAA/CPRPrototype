using AdvancedTimer.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CprPrototype.Model
{
    public class AlgorithmTimer
    {
        #region Properties 

        // Gain access to timers via DependencyService from XForms
        private IAdvancedTimer totalTimer = DependencyService.Get<IAdvancedTimer>();
        
        private int interval;

        /// <summary>
        /// Start DateTime.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Accessor for the totalTimer object.
        /// </summary>
        public IAdvancedTimer Timer { get { return totalTimer; } }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public AlgorithmTimer(int interval, bool init = true)
        {
            this.interval = interval;

            if (init)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Initialize totalTimer.
        /// </summary>
        public void Initialize()
        {
            totalTimer.initTimer(interval, TimerElapsedEvent, true);
        }

        #endregion

        #region Events & Handlers

        // Event for ViewModel to subscribe to
        public event EventHandler TimerElapsedEvent;

        /// <summary>
        /// Fire our TimerElapsedEvent.
        /// </summary>
        protected virtual void OnTimerElapsedEvent()
        {
            if (TimerElapsedEvent != null)
            {
                TimerElapsedEvent(this, EventArgs.Empty);
            }
        }

        #endregion

        /// <summary>
        /// Starts the totalTimer and sets DateTime property.
        /// </summary>
        public void StartTimer()
        {
            StartTime = DateTime.Now;
            totalTimer.startTimer();
        }
    }
}
