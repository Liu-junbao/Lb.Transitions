using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lb.Transitions.TransitionWipes
{
    public class RollWipe:CustomWipe
    {
        public override void Wipe(TransitionerSlide fromSlide, TransitionerSlide toSlide, Point origin, IZIndexController zIndexController)
        {
            if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
            if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            zIndexController.Stack(fromSlide, toSlide);

            if (FromStoryboard != null)
                BeginStoryboard(FromStoryboard, FromStyle, fromSlide, zIndexController, () =>
                {
                    fromSlide.Visibility = Visibility.Hidden;
                    zIndexController.Stack(toSlide, fromSlide);
                });
            if (ToStoryboard != null)
                BeginStoryboard(ToStoryboard, ToStyle, toSlide, zIndexController, () =>
                {
                    fromSlide.Visibility = Visibility.Hidden;
                    zIndexController.Stack(toSlide, fromSlide);
                });
        }
    }
}
