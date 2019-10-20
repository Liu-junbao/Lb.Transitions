using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Lb.Transitions.TransitionWipes
{
    [StyleTypedProperty(Property = "OldStyle", StyleTargetType = typeof(TransitionerSlide))]
    [StyleTypedProperty(Property = "OldStyle", StyleTargetType = typeof(TransitionerSlide))]
    [TypeConverter(typeof(CustomWipeTypeConverter))]
    public class CustomWipe :BaseWipe
    {
        public Style FromStyle { get; set; }
        public Style ToStyle { get; set; }
        public Storyboard FromStoryboard { get; set; }
        public Storyboard ToStoryboard { get; set; }

        public override void Wipe(TransitionerSlide fromSlide, TransitionerSlide toSlide, Point origin, IZIndexController zIndexController)
        {
            if (fromSlide == null) throw new ArgumentNullException(nameof(fromSlide));
            if (toSlide == null) throw new ArgumentNullException(nameof(toSlide));
            if (zIndexController == null) throw new ArgumentNullException(nameof(zIndexController));

            if (FromStoryboard != null)
                BeginStoryboard(FromStoryboard, FromStyle, fromSlide,zIndexController, () => fromSlide.Visibility = Visibility.Hidden);
            if (ToStoryboard != null)
                BeginStoryboard(ToStoryboard, ToStyle, toSlide, zIndexController, () => fromSlide.Visibility = Visibility.Hidden);

            zIndexController.Stack(toSlide, fromSlide);
        }

        protected void BeginStoryboard(Storyboard storyboard, Style style, TransitionerSlide element, IZIndexController zIndexController, Action endAction = null)
        {

            var settings = style.Setters.Where(i => i is Setter).Select(i => (Setter)i);
            var origin = settings.FirstOrDefault(i => i.Property == FrameworkElement.RenderTransformOriginProperty)?.Value;
            if (origin != null) element.RenderTransformOrigin = (Point)origin;
            element.RenderTransform = (Transform)settings.FirstOrDefault(i => i.Property == FrameworkElement.RenderTransformProperty)?.Value;
            element.OpacityMask = (Brush)settings.FirstOrDefault(i => i.Property == FrameworkElement.OpacityMaskProperty)?.Value;
            element.BitmapEffect = (BitmapEffect)settings.FirstOrDefault(i => i.Property == FrameworkElement.BitmapEffectProperty)?.Value;
            element.Effect = (Effect)settings.FirstOrDefault(i => i.Property == FrameworkElement.EffectProperty)?.Value;

            this.CompletedWithEnd(storyboard, zIndexController, () =>
              {
                  storyboard.Seek(element, TimeSpan.Zero, TimeSeekOrigin.BeginTime);
                  storyboard.Remove(element);
                  element.RenderTransformOrigin = new Point();
                  element.RenderTransform = null;
                  element.OpacityMask = null;
                  element.BitmapEffect = null;
                  element.Effect = null;

                  endAction?.Invoke();
              });

            storyboard.Begin(element, true);
        }
    }

    public enum CustomKind
    {
        Checkerboard,
        DiagonalWipe,
        Diamonds,
        Dots,
        DoubleRotateWipe,
        FadeAndBlur,
        FadeAndGrow,
        HorizontalBlinds,
        HorizontalWipe,
        Melt,
        Roll,
        RotateWipe,
        Star,
        VerticalBlinds,
        VerticalWipe
    }

    [MarkupExtensionReturnType(typeof(CustomWipe))]
    public class CustomWipeExtension : MarkupExtension
    {
        public CustomWipeExtension() { }
        public CustomWipeExtension(CustomKind kind)
        {
            Kind = kind;
        }
        [ConstructorArgument("kind")]
        public CustomKind Kind { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return CustomWipeTypeConverter.GetCustomWipe(Kind);
        }
    }
    [MarkupExtensionReturnType(typeof(string))]
    public class CustomWipeKeyExtension : MarkupExtension
    {
        public CustomWipeKeyExtension() { }
        public CustomWipeKeyExtension(CustomKind kind)
        {
            Kind = kind;
        }
        [ConstructorArgument("kind")]
        public CustomKind Kind { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetName(typeof(CustomKind),Kind);
        }
    }
    public class CustomWipeTypeConverter:TypeConverter
    {
        private static ResourceDictionary _customWipeResources = Application.LoadComponent(new Uri("/Lb.Transitions;component/Themes/Themes.CustomWipe.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
        private static TValue GetValue<TValue>(string name) where TValue : class => _customWipeResources[name] as TValue;
        public static CustomWipe GetCustomWipe(CustomKind kind)
        {
            return GetValue<CustomWipe>(Enum.GetName(typeof(CustomKind),kind));
        }

        public static CustomWipe[] GetCustomWipes()
        {
            return Enum.GetNames(typeof(CustomKind)).Select(i => GetValue<CustomWipe>(i)).ToArray();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            CustomKind kind;
            if (s != null && Enum.TryParse(s, out kind)) return GetCustomWipe(kind);
            return base.ConvertFrom(context, culture, value);
        }
    }


   
}
