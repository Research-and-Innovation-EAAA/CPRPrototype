using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CprPrototype.Model
{
    /// <summary>
    /// The AssessmentStep class represents the initial step of the CPR Algorithm
    /// where the user must choose between Shockable or Unshocable rythm. In case
    /// of restored circulation the user can exist the algorithm loop.
    /// </summary>
    public class AssessmentStep : AlgorithmStep, IDisposable
    {
        /// <summary>
        /// This is the step when the circulation is restored.
        /// </summary>
        public AlgorithmStep CircRestoredStep { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// The Previous step will always remain null.
        /// The Next step will be deciced when the user selected
        /// the style of the rythm. (Shockable / NonShockable)
        /// </remarks>
        /// <param name="name">Name of the step</param>
        /// <param name="description">Description of the step</param>
        /// <param name="previous">null</param>
        /// <param name="next">null</param>
        /// <param name="circRestoredStep">Circulation Restored step</param>
        public AssessmentStep(string name, string description) : base(name, description)
        {
            
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public override void Dispose()
        {
            this.Name = null;
            this.Description = null;
            this.NextStep = null;
            this.PreviousStep = null;
            this.RythmStyle = null;
            this.CircRestoredStep = null;
        }
    }

    public enum RythmStyle
    {
        Shockable = 0,
        NonShockable = 1
    }
}
