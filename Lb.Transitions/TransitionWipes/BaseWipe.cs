using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Lb.Transitions.TransitionWipes
{
    public abstract class BaseWipe : ITransitionWipe
    {
        public abstract void Wipe(TransitionerSlide fromSlide, TransitionerSlide toSlide, Point origin, IZIndexController zIndexController);

        protected void CompletedWithEnd(Timeline timeline,IZIndexController zIndexController,Action completedAction)
        {
            EventHandler endHandler = null;
            endHandler = (s, e) =>
            {
                completedAction?.Invoke();
                timeline.Completed -= endHandler;
                zIndexController.Activated -= endHandler;
            };
            timeline.Completed += endHandler;
            zIndexController.Activated += endHandler;        
        }
    }
}
