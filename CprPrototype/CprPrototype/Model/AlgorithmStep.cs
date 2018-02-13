using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CprPrototype.Model
{
    /// <summary>
    /// The AlgorithmStep class represents a step in the CPR algorithm.
    /// </summary>
    public class AlgorithmStep : IDisposable
    {
        #region Properties

        /// <summary>
        /// Name of the Step.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the Step.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Previous Step.
        /// </summary>
        public AlgorithmStep PreviousStep { get; set; }

        /// <summary>
        /// Next Step.
        /// </summary>
        public AlgorithmStep NextStep { get; set; }

        /// <summary>
        /// Represents the style of the assessed rythm either shockable or non-shockable.
        /// </summary>
        public RythmStyle RythmStyle { get; set; }

        #endregion

        #region Construction & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmStep"/> class.
        /// </summary>
        /// <param name="name">Name of the step</param>
        /// <param name="description">Description of the step</param>

        public AlgorithmStep(string name, string description)
        {
            Name = name;
            Description = description;
        }

        ///// <summary>
        ///// CTOR.
        ///// </summary>
        //public AlgorithmStep()
        //{
        //    // Empty
        //}

        /// <summary>
        /// IDisposible implementation.
        /// </summary>
        public virtual void Dispose()
        {
            Name = null;
            Description = null;
            NextStep = null;
            PreviousStep = null;
        }

        #endregion
    }
}
