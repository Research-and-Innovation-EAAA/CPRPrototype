using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CprPrototype.View
{
    public class ExDrugsModel
    {
        public List<ExtraDrugs> Exdrugs { get; set; }
        public ExDrugsModel()
        {
            Exdrugs = new ExtraDrugs().GetExtraDrugs();
        }
    }
}
