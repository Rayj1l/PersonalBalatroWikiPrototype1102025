using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balatro_Deck_Maker_Prototype_1.Classes
{
    // Enter Effect Type (Write as seen) \n+$. \n+C, \n+M, \n*M, \nEffect, \nRetrigger"
    public enum EffectTypes
    {
        None = 0,
        Money,
        Chips,
        Add,
        Multiply,
        Retrigger,
        Misc
    }
}
