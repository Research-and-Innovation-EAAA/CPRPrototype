using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CprPrototype.Model
{
   public enum InteractionMode
   {
        Silent,
        Sound
   }

   public enum StepSize
    {
        Small,
        Big
    }

   public enum DrugType
    {
        Adrenalin,
        Amiodaron,
        Bikarbonat,
        Calcium,
        Magnesium
    }

    public enum DrugDoseTarget
    {
        Adult,
        Children
    }
}
