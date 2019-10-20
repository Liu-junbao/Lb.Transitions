using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb.Transitions
{
    public class TransitionWipeStrategy:List<ITransitionWipeStrategy>,ITransitionWipeStrategy
    {
        public bool Match(Transitioner transitioner, TransitionerSlide oldSlide, TransitionerSlide newSlide, int oldIndex, int newIndex, int count, out ITransitionWipe wipe)
        {
            foreach (var item in this)
            {
                if (item.Match(transitioner, oldSlide, newSlide, oldIndex, newIndex, count, out wipe))
                {
                    return true;
                }
            }
            wipe = null;
            return false;
        }
    }
}
