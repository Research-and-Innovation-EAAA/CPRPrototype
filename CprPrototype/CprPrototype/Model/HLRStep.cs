using System;

namespace CprPrototype.Model
{
    /// <summary>
    /// The HLR class represents the actual 2 minutes of resuscitation
    /// It's mainly used to differentiate from the AssessmentStep class.
    /// <remark>
    /// {PWM} A possible solution could be to employ an enum for this task, to simplify the project since it currently does nothinf special.
    /// </remark>
    /// </summary>
    public class HLRStep : AlgorithmStep, IDisposable
    {
        public HLRStep(string name, string description) : base(name, description)
        {

        }
    }
}
