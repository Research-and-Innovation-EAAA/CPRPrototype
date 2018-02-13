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
        /// Gets or sets the name of the <see cref="AlgorithmStep"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="AlgorithmStep"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the previous <see cref="AlgorithmStep"/>.
        /// </summary>
        public AlgorithmStep PreviousStep { get; set; }

        /// <summary>
        /// Gets or sets the nest <see cref="AlgorithmStep"/>
        /// </summary>
        public AlgorithmStep NextStep { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AlgorithmStep"/>'s <see cref="Model.RythmStyle"/>
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
