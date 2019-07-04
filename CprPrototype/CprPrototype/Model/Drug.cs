using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace CprPrototype.Model
{
    /// <summary>
    /// Represents the definition of a drug and its dosage
    /// </summary>
    public class Drug
    {
        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Model.DrugType"/> of the drug
        /// </summary>
        public DrugType DrugType { get; set; }

        /// <summary>
        /// Gets or sets the collection of different doses the drug is given in
        /// </summary>
        public List<DrugShot> DosesCollection { get; set; }
        
        /// <summary>
        /// Gets or sets the value indicating whether the drug has been injected
        /// </summary>
        public bool IsInjected { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> for when the drug was last administered
        /// </summary>
        public DateTime TimeOfLatestInjection { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> for preparation of the drug
        /// </summary>
        public TimeSpan PreparationTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> for injection of the drug
        /// </summary>
        public TimeSpan InjectionTime { get; set; }
        
        #endregion

        #region Contruction & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="Drug"/> class
        /// </summary>
        /// <param name="drugType">Name of the type of drug</param>
        /// <param name="preparationTime">Time needed for preparation</param>
        public Drug(DrugType drugType, TimeSpan preparationTime, TimeSpan injectionTime)
        {
            DrugType = drugType;
            DosesCollection = new List<DrugShot>();
            IsInjected = false;
            PreparationTime = preparationTime;
            InjectionTime = injectionTime;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets the DrugShot based on total cycles elapsed.
        /// </summary>
        /// <param name="totalCyclesElapsed">The total amount of cycles elapsed</param>
        /// <param name="rythmStyle">Shockable/Non-Shockable</param>
        /// <returns>Returns a drugshot for the current situation. Null if there isn't one.</returns>
        public DrugShot GetDrugShot(int totalCyclesElapsed, RythmStyle rythmStyle)
        {
            DrugShot result = null;

            // All drug types:
            switch (DrugType)
            {
                case DrugType.Epinephrine:

                    // Address adrenaline immediatelly on the
                    // first non-shockable step, then every
                    // 3-5 minutes.
                    switch (rythmStyle)
                    {
                        case RythmStyle.NonShockable:
                            // Address immediatelly in NShockable (Called once)
                            if (IsInjected == false)
                            {
                                IsInjected = true;
                                TimeOfLatestInjection = DateTime.Now.Add(TimeSpan.FromMinutes(2));
                                result = DosesCollection.Find(d => d.Dose == "1mg");
                            }
                            // Then every 3-5 minutes
                            else if (totalCyclesElapsed >= 1 && DateTime.Now.Subtract(TimeOfLatestInjection).TotalMinutes >= 3)
                            {
                                TimeOfLatestInjection = DateTime.Now.Add(TimeSpan.FromMinutes(3));
                                result = DosesCollection.Find(d => d.Dose == "1mg");
                            }
                            break;
                        case RythmStyle.Shockable:
                            // Address first time in Shockable after 3 cycles
                            if (totalCyclesElapsed >= 1 && IsInjected == false)
                            {
                                IsInjected = true;
                                TimeOfLatestInjection = DateTime.Now.Add(PreparationTime);
                                result = DosesCollection.Find(d => d.Dose == "1mg");
                            }
                            // Then every 3-5 minutes
                            else if (totalCyclesElapsed > 3 && DateTime.Now.Subtract(TimeOfLatestInjection).TotalMinutes >= 3)
                            {
                                result = DosesCollection.Find(d => d.Dose == "1mg");
                                TimeOfLatestInjection = DateTime.Now.Add(TimeSpan.FromMinutes(3));
                            }
                            break;
                    }
                    break;
                case DrugType.Amiodarone:
                    if (rythmStyle == RythmStyle.Shockable)
                    {
                        // Give at 3rd cycle, smaller dose if
                        // cycles >= 5.
                        if (totalCyclesElapsed == 1 && totalCyclesElapsed % 1 == 0 && IsInjected == false)
                        {
                            result = DosesCollection.Find(d => d.Dose == "300ml");
                            TimeOfLatestInjection = DateTime.Now.Add(PreparationTime);
                        }
                        // Address after 5 additional cycles, 
                        // rather than 5 in total
                        else if (totalCyclesElapsed >= 5 && IsInjected == true && ((totalCyclesElapsed - 3) % 5) == 0)
                        {
                            result = DosesCollection.Find(d => d.Dose == "150ml");
                            TimeOfLatestInjection = DateTime.Now.Add(PreparationTime);
                        }
                    }
                    break;
                case DrugType.Bicarbonate:
                case DrugType.Calcium:
                    result = DosesCollection[0];
                    TimeOfLatestInjection = DateTime.Now.Add(PreparationTime);
                    break;
            }

            return result;
        }
        
        #endregion
    }
}
