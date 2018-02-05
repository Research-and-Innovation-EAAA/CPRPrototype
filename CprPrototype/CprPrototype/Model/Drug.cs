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
    /// The Drug class contains the definition of a drug,
    /// including it's dosage.
    /// </summary>
    public class Drug
    {
        public DrugType DrugType { get; set; }
        public List<DrugShot> DosesCollection { get; set; }
        public bool IsInjected { get; set; }
        public DateTime TimeOfLatestInjection { get; set; }
        public TimeSpan PrepTime { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="drugType">Name of the type of drug</param>
        /// <param name="prepTime">How much time it needs for preparation</param>
        public Drug(DrugType drugType, TimeSpan prepTime)
        {
            IsInjected = false;
            DrugType = drugType;
            PrepTime = prepTime;
            DosesCollection = new List<DrugShot>();
        }

        /// <summary>
        /// Gets the correct DrugShot based on total cycles.
        /// </summary>
        /// <param name="totalCycles">The total amount of cycles elapsed</param>
        /// <param name="rythmStyle">Shockable/Non-Shockable</param>
        /// <returns>Returns a drugshot for the current situation. Null if there isn't one.</returns>
        public DrugShot GetDrugShot(int totalCycles, RythmStyle rythmStyle)
        {
            DrugShot result = null;

            // All drug types:
            switch (DrugType)
            {
                case DrugType.Adrenalin:

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
                            else if (totalCycles >= 1 && DateTime.Now.Subtract(TimeOfLatestInjection).TotalMinutes >= 3)
                            {
                                TimeOfLatestInjection = DateTime.Now.Add(TimeSpan.FromMinutes(3));
                                result = DosesCollection.Find(d => d.Dose == "1mg");
                            }
                            break;
                        case RythmStyle.Shockable:
                            // Address first time in Shockable after 3 cycles
                            if (totalCycles >= 2 && IsInjected == false)
                            {
                                IsInjected = true;
                                TimeOfLatestInjection = DateTime.Now.Add(PrepTime);
                                result = DosesCollection.Find(d => d.Dose == "1mg");
                            }
                            // Then every 3-5 minutes
                            else if (totalCycles > 3 && DateTime.Now.Subtract(TimeOfLatestInjection).TotalMinutes >= 3)
                            {
                                result = DosesCollection.Find(d => d.Dose == "1mg");
                                TimeOfLatestInjection = DateTime.Now.Add(TimeSpan.FromMinutes(3));
                            }
                            break;
                    }
                    break;
                case DrugType.Amiodaron:
                    if (rythmStyle == RythmStyle.Shockable)
                    {
                        // Give at 3rd cycle, smaller dose if
                        // cycles >= 5.
                        if (totalCycles >= 3 && totalCycles % 3 == 0 && IsInjected == false)
                        {
                            result = DosesCollection.Find(d => d.Dose == "300ml");
                            TimeOfLatestInjection = DateTime.Now.Add(PrepTime);
                        }
                        // Address after 5 additional cycles, 
                        // rather than 5 in total
                        else if (totalCycles >= 5 && IsInjected == true && ((totalCycles - 3) % 5) == 0)
                        {
                            result = DosesCollection.Find(d => d.Dose == "150ml");
                            TimeOfLatestInjection = DateTime.Now.Add(PrepTime);
                        }
                    }
                    break;
                case DrugType.Bikarbonat:
                case DrugType.Calcium:
                    result = DosesCollection[0];
                    TimeOfLatestInjection = DateTime.Now.Add(PrepTime);
                    break;
            }

            return result;
        }
    }
}
