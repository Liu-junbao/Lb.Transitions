using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Lb.Transitions.TransitionWipes
{
    public class FadeWipe :BaseWipe
    {
        private readonly SineEase _sineEase = new SineEase();

        /// <summary>
        /// Duration of the animation
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0.5);

        public override void Wipe(TransitionerSlide fromSlide, TransitionerSlide toSlide, Point origin, IZIndexController zIndexController)
        {
            if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
            if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            // Set up time points
            var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            var endKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(Duration.TotalSeconds/2));

            // From
            var fromAnimation = new DoubleAnimationUsingKeyFrames();
            fromAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1, zeroKeyTime));
            fromAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, endKeyTime, _sineEase));

            // To
            var toAnimation = new DoubleAnimationUsingKeyFrames();
            toAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, zeroKeyTime));
            toAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, endKeyTime, _sineEase));

            // Preset
            fromSlide.Opacity = 1;
            toSlide.Opacity = 0;

            // Set up events
            this.CompletedWithEnd(toAnimation,zIndexController,()=>
            {
                toSlide.BeginAnimation(UIElement.OpacityProperty, null);
                fromSlide.Opacity = 1;
                toSlide.Opacity = 1;
                fromSlide.Visibility = Visibility.Hidden;
            });

            this.CompletedWithEnd(fromAnimation,zIndexController,()=>
            {
                fromSlide.BeginAnimation(UIElement.OpacityProperty, null);
                fromSlide.Opacity = 1;
                toSlide.BeginAnimation(UIElement.OpacityProperty, toAnimation);
                fromSlide.Visibility = Visibility.Hidden;
            });

            // Animate
            fromSlide.BeginAnimation(UIElement.OpacityProperty, fromAnimation);
            zIndexController.Stack(toSlide, fromSlide);
        }
    }
}