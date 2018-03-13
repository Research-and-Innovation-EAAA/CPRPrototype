using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CprPrototype.View
{
    public class ExDrugs
    {
        public string DrugName { get; set; }
        public string DrugUsage { get; set; }
        public string DrugID { get; set; }

        public List<ExDrugs> GetExtraDrugs()
        {
            List<ExDrugs> _extraDrugs = new List<ExDrugs>()
            {
               new ExDrugs()
               {
                   DrugName = "Adrenalin",DrugUsage = "1 mg Bolus hvert 3.-5 min.", DrugID = "1"
               },
               new ExDrugs()
               {
                   DrugName = "Amiodaron", DrugUsage = "300 mg bolus efter 3.stød Evt. gentages 150 mg bolus efter 5.stød", DrugID = "2"
               },
               new ExDrugs()
               {
                   DrugName = "Bikarbonat",DrugUsage = "50 ml 8,4 % (50 mmol) natriumbikarbonat bolus gentages ved behov", DrugID = "3"
               },
                new ExDrugs()
               {
                   DrugName = "Calcium",DrugUsage = "10 ml (5 mmol) calciumchlorid 0,5 mmol/ml bolus gentages ved behov", DrugID = "4"
               },
                 new ExDrugs()
               {
                   DrugName = "Magnesium",DrugUsage = "4 ml (8 mmol) magnesiumsulfat/magnesiumklorid 2mmol/ml over 1-2 min. Evt. gentages efter 10-15 min.", DrugID = "5"
               }
            };

            return _extraDrugs;
        }
    }
}