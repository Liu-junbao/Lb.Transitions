using Lb.Transitions.TransitionWipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Lb.Transitions.TransitionWipeSubjects
{
    [ContentProperty(nameof(Wipes))]
    public class RandomStrategy : ITransitionWipeStrategy
    {
        private List<ITransitionWipe> _wipes;
        private Random _random;
        public RandomStrategy()
        {
            _wipes = new List<ITransitionWipe>()
            {
                new CircleWipe(),
                new FadeWipe(),
                new RollWipe(),
                new SlideOutWipe(),
                new SlideWipe(),
            };
            _wipes.AddRange(CustomWipeTypeConverter.GetCustomWipes());
            Wipes = new List<ITransitionWipe>();
            _random = new Random();
        }
        public List<ITransitionWipe> Wipes { get; }
        public bool Match(Transitioner transitioner, TransitionerSlide oldSlide, TransitionerSlide newSlide, int oldIndex, int newIndex, int count, out ITransitionWipe wipe)
        {
            if (Wipes.Count > 0)
                wipe = Wipes[_random.Next(0, Wipes.Count - 1)];
            else
                wipe = _wipes[_random.Next(0, _wipes.Count - 1)];
            return true;
        }
    }
}
