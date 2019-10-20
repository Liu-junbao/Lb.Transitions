using System;

namespace Lb.Transitions
{
    public interface IZIndexController
    {
        event EventHandler Activated;
        void Stack(params TransitionerSlide[] highestToLowest);
    }
}