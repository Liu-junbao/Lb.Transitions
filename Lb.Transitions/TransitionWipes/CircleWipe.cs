using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Lb.Transitions.TransitionWipes
{
    public class CircleWipe : BaseWipe
    {
        public override void Wipe(TransitionerSlide fromSlide, TransitionerSlide toSlide, Point origin, IZIndexController zIndexController)
        {
            if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
            if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            var horizontalProportion = Math.Max(1.0 - origin.X, 1.0 * origin.X);
            var verticalProportion = Math.Max(1.0 - origin.Y, 1.0 * origin.Y);
            var radius = Math.Sqrt(Math.Pow(toSlide.ActualWidth * horizontalProportion, 2) + Math.Pow(toSlide.ActualHeight * verticalProportion, 2));

            var scaleTransform = new ScaleTransform(0, 0);
            var translateTransform = new TranslateTransform(toSlide.ActualWidth * origin.X, toSlide.ActualHeight * origin.Y);
            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            var ellipseGeomotry = new EllipseGeometry()
            {
                RadiusX = radius,
                RadiusY = radius,
                Transform = transformGroup
            };

            toSlide.SetCurrentValue(UIElement.ClipProperty, ellipseGeomotry);
            zIndexController.Stack(toSlide, fromSlide);

            var zeroKeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            var midKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));
            var endKeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(400));

            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, zeroKeyTime));
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, midKeyTime));
            opacityAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, endKeyTime));

            var scaleAnimation = new DoubleAnimationUsingKeyFrames();
            scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(0, zeroKeyTime));
            scaleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame(1, endKeyTime));

            this.CompletedWithEnd(opacityAnimation, zIndexController, () =>
              {
                  fromSlide.BeginAnimation(UIElement.OpacityProperty, null);
                  fromSlide.Opacity = 1;
                  fromSlide.Visibility = Visibility.Hidden;
              });

            this.CompletedWithEnd(scaleAnimation, zIndexController, () =>
              {
                  toSlide.SetCurrentValue(UIElement.ClipProperty, null);
                  fromSlide.BeginAnimation(UIElement.OpacityProperty, null);
                  fromSlide.RenderTransform = null;
                  toSlide.RenderTransform = null;
                  fromSlide.Opacity = 1;
                  fromSlide.Visibility = Visibility.Hidden;
              });

            fromSlide.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
    }
}