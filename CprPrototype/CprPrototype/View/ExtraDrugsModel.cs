using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CprPrototype.View
{
    public class ExDrugsModel
    {
        public List<ExDrugs> Exdrugs { get; set; }
        public ExDrugsModel()
        {
            Exdrugs = new ExDrugs().GetExtraDrugs();
        }
    }
}
