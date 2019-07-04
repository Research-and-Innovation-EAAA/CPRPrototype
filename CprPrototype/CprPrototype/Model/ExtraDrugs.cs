using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CprPrototype.Model;
using CprPrototype.Service;

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
                   DrugName = DrugType.Epinephrine.ToString(),DrugUsage = Translator.Instance.Translate("EpinephrineUsage")
               },
               new ExtraDrugs()
               {
                   DrugName = DrugType.Amiodarone.ToString(), DrugUsage = Translator.Instance.Translate("AmiodaroneUsage")
               },
               new ExtraDrugs()
               {
                   DrugName = DrugType.Bicarbonate.ToString(),DrugUsage = Translator.Instance.Translate("BicarbonateUsage")
               },
                new ExtraDrugs()
               {
                   DrugName = DrugType.Calcium.ToString(),DrugUsage = Translator.Instance.Translate("CalciumUsage")
               },
                 new ExtraDrugs()
               {
                   DrugName = DrugType.Magnesium.ToString(),DrugUsage = Translator.Instance.Translate("MagnesiumUsage")
               }
            };

            return _extraDrugs;
        }
    }
}