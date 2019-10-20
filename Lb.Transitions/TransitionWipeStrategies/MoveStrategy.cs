using Lb.Transitions.TransitionWipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb.Transitions.TransitionWipeStrategies
{
    public class MoveStrategy : ITransitionWipeStrategy
    {
        public MoveStrategy()
        {
            ForwardWipe = new SlideWipe() { Direction = SlideDirection.Left };
            BackwardWipe = new SlideWipe() { Direction = SlideDirection.Right };
        }
        public ITransitionWipe ForwardWipe { get; set; }
        public ITransitionWipe BackwardWipe { get; set; }
        public bool Match(Transitioner transitioner, TransitionerSlide oldSlide, TransitionerSlide newSlide, int oldIndex, int newIndex, int count, out ITransitionWipe wipe)
        {
            wipe = newIndex > oldIndex ? ForwardWipe : BackwardWipe;
            return true;
        }
    }
}
