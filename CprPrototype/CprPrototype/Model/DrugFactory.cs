using System;
using System.Collections.Generic;

namespace CprPrototype.Model
{
    /// <summary>
    /// The DrugFactory class is responsible for the construction
    /// of all drugs and their dosages.
    /// </summary>
    public class DrugFactory
    {
        /// <summary>
        /// Creates a list of Drugs, including dosage for each drug.
        /// </summary>
        /// <param name="prepTimeMinutes">Optional - Drug Prep Time in Minutes. (Default = 5.0)</param>
        /// <returns>List of Drugs</returns>
        public List<Drug> CreateDrugs(double prepTimeMinutes = 5)
        {
            var result = new List<Drug>();
            var prepTime = TimeSpan.FromMinutes(prepTimeMinutes);

            // Adrenalin
            var adrenalinDrug = new Drug(DrugType.Adrenalin, prepTime);
            adrenalinDrug.Doses.Add(new DrugShot(adrenalinDrug, DrugDoseTarget.Adult, "1mg"));
            result.Add(adrenalinDrug);

            // Amiodoran
            var amiodoranDrug = new Drug(DrugType.Amiodaron, prepTime);
            amiodoranDrug.Doses.Add(new DrugShot(amiodoranDrug, DrugDoseTarget.Adult, "300ml"));
            amiodoranDrug.Doses.Add(new DrugShot(amiodoranDrug, DrugDoseTarget.Adult, "150ml"));
            result.Add(amiodoranDrug);

            // TODO: Add extra drugs

            return result;
        }
    }
}
