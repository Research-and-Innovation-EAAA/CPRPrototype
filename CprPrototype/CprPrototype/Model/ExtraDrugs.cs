using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CprPrototype.Model;

namespace CprPrototype.View
{
    public class ExtraDrugs
    {
        public string DrugName { get; set; }
        public string DrugUsage { get; set; }

        public List<ExtraDrugs> GetExtraDrugs()
        {
            List<ExtraDrugs> _extraDrugs = new List<ExtraDrugs>()
            {
               new ExtraDrugs()
               {
                   DrugName = DrugType.Adrenalin.ToString(),DrugUsage = "1 mg Bolus hvert 3.-5 min."
               },
               new ExtraDrugs()
               {
                   DrugName = DrugType.Amiodaron.ToString(), DrugUsage = "300 mg bolus efter 3.stød Evt. gentages 150 mg bolus efter 5.stød"
               },
               new ExtraDrugs()
               {
                   DrugName = DrugType.Bikarbonat.ToString(),DrugUsage = "50 ml 8,4 % (50 mmol) natriumbikarbonat bolus gentages ved behov"
               },
                new ExtraDrugs()
               {
                   DrugName = DrugType.Calcium.ToString(),DrugUsage = "10 ml (5 mmol) calciumchlorid 0,5 mmol/ml bolus gentages ved behov"
               },
                 new ExtraDrugs()
               {
                   DrugName = DrugType.Magnesium.ToString(),DrugUsage = "4 ml (8 mmol) magnesiumsulfat/magnesiumklorid 2mmol/ml over 1-2 min. Evt. gentages efter 10-15 min."
               }
            };

            return _extraDrugs;
        }
    }
}