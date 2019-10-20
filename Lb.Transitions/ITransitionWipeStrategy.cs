using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb.Transitions
{
    public interface ITransitionWipeStrategy
    {
        bool Match(Transitioner transitioner,TransitionerSlide oldSlide,TransitionerSlide newSlide,int oldIndex,int newIndex,int count,out ITransitionWipe wipe);
    }
}
