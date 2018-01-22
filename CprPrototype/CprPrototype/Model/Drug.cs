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
        public List<DrugShot> Doses { get; set; }
        public bool Injected { get; set; }
        public DateTime LastInjection { get; set; }
        public TimeSpan PrepTime { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Drug(DrugType type, TimeSpan prepTime)
        {
            Injected = false;
            DrugType = type;
            PrepTime = prepTime;
            Doses = new List<DrugShot>();
        }

        /// <summary>
        /// Gets the correct DrugShot based on total cycles.
        /// </summary>
        /// <param name="cycles">Total Cycles</param>
        /// <param name="style">Shockable/Non-Shockable</param>
        /// <returns>Returns a drugshot for the current situation. Null if there isn't one.</returns>
        public DrugShot GetDrugShot(int cycles, RythmStyle style)
        {
            DrugShot result = null;

            switch (DrugType)
            {
                case DrugType.Adrenalin:

                    //result = Doses[0];

                    // Address adrenaline immediatelly on the
                    // first non-shockable step, then every
                    // 3-5 minutes.
                    switch (style)
                    {
                        case RythmStyle.NonShockable:
                            // Address immediatelly in NShockable
                            if (Injected == false)
                            {
                                Injected = true;
                                LastInjection = DateTime.Now.Add(TimeSpan.FromMinutes(2));
                                result = Doses[0];
                            }
                            // Then every 3-5 minutes
                            else if (cycles >= 1 && DateTime.Now.Subtract(LastInjection).TotalMinutes >= 3)
                            {
                                LastInjection = DateTime.Now.Add(TimeSpan.FromMinutes(3));
                                result = Doses[0];
                            }
                            break;
                        case RythmStyle.Shockable:
                            // Address first time in Shockable after 3 cycles
                            if (cycles >= 2 && Injected == false)
                            {
                                Injected = true;
                                LastInjection = DateTime.Now.Add(PrepTime);
                                result = Doses[0];
                            }
                            // Then every 3-5 minutes
                            else if (cycles > 3 && DateTime.Now.Subtract(LastInjection).TotalMinutes >= 3)
                            {
                                result = Doses[0];
                                LastInjection = DateTime.Now.Add(TimeSpan.FromMinutes(3));
                            }
                            break;
                    }
                    break;
                case DrugType.Amiodaron:
                    if (style == RythmStyle.Shockable)
                    {
                        // Give at 3rd cycle, smaller dose if
                        // cycles >= 5.
                        if (cycles >= 3 && cycles % 3 == 0 && Injected == false)
                        {
                            result = Doses[0];
                            LastInjection = DateTime.Now.Add(PrepTime);
                        }
                        // Address after 5 additional cycles, 
                        // rather than 5 in total
                        else if (cycles >= 5 && Injected == true && ((cycles - 3) % 5) == 0)
                        {
                            result = Doses[1];
                            LastInjection = DateTime.Now.Add(PrepTime);
                        }
                    }
                    break;
                case DrugType.Bikarbonat:
                case DrugType.Calcium:
                    result = Doses[0];
                    LastInjection = DateTime.Now.Add(PrepTime);
                    break;
            }

            return result;
        }
    }
}
