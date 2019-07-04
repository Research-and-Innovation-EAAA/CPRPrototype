using System;
using System.Collections.Generic;

namespace CprPrototype.Model
{
    /// <summary>
    /// Provides a Factory who is responsible for the construction
    /// of all drugs and their dosages.
    /// </summary>
    public class DrugFactory
    {
        #region Methods

        /// <summary>
        /// Creates a list of Drugs, including dosages for each drug.
        /// </summary>
        /// <param name="preparationTimeMinutes">Drug preparation time in minutes.</param>
        /// <param name="injectionTimeMinutes">Drug injection time in minutes</param>
        /// <returns>List of Drugs</returns>
        public List<Drug> CreateDrugs(double preparationTimeMinutes = 3, double injectionTimeMinutes = 2)
        {
            var newDrugList = new List<Drug>();
            var preparationTime = TimeSpan.FromMinutes(preparationTimeMinutes);
            var injectionTime = TimeSpan.FromMinutes(injectionTimeMinutes);

            //========================================================================
            // Adrenalin init
            //========================================================================

            var adrenalinDrug = new Drug(DrugType.Epinephrine, preparationTime, injectionTime);
            adrenalinDrug.DosesCollection.Add(new DrugShot(adrenalinDrug, "1mg"));
            newDrugList.Add(adrenalinDrug);

            //========================================================================
            // Amiodaron init
            //========================================================================

            var amiodoranDrug = new Drug(DrugType.Amiodarone, preparationTime, injectionTime);
            amiodoranDrug.DosesCollection.Add(new DrugShot(amiodoranDrug, "300ml"));
            amiodoranDrug.DosesCollection.Add(new DrugShot(amiodoranDrug, "150ml"));
            newDrugList.Add(amiodoranDrug);

            //========================================================================
            // TODO: Extras
            //========================================================================
            return newDrugList;
        }
        
        #endregion
    }
}
