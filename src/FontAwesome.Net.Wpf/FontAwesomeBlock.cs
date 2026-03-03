using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FontAwesome.Net.Wpf
{
    public class FontAwesomeBlock
        : TextBlock, ISpinnable, IRotatable, IFlippable
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(IFontAwesomeIcon), typeof(FontAwesomeBlock), new PropertyMetadata(null, OnIconChanged));

        public static readonly DependencyProperty IconStyleProperty = DependencyProperty.Register(
            nameof(IconStyle), typeof(IFontAwesomeIconStyle), typeof(FontAwesomeBlock), new PropertyMetadata(null, OnIconStyleChanged));

        public static readonly DependencyProperty SpinProperty = DependencyProperty.Register(
            nameof(Spin), typeof(bool), typeof(FontAwesomeBlock), new PropertyMetadata(false, OnSpinPropertyChanged, SpinCoerceValue));
        
        public static readonly DependencyProperty SpinDurationProperty = DependencyProperty.Register(
            nameof(SpinDuration), typeof(double), typeof(FontAwesomeBlock), new PropertyMetadata(1d, OnSpinDurationChanged, SpinDurationCoerceValue));

        public static readonly DependencyProperty ReverseSpinDirectionProperty = DependencyProperty.Register(
            nameof(ReverseSpinDirection), typeof(bool), typeof(FontAwesomeBlock), new PropertyMetadata(false, OnReverseSpinDirectionChanged));

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(
            nameof(Rotation), typeof(double), typeof(FontAwesomeBlock), new PropertyMetadata(0d, OnRotationChanged, RotationCoerceValue));
        
        public static readonly DependencyProperty FlipOrientationProperty = DependencyProperty.Register(
            nameof(FlipOrientation), typeof(FlipOrientation), typeof(FontAwesomeBlock), new PropertyMetadata(FlipOrientation.None, OnFlipOrientationChanged));

        static FontAwesomeBlock()
        {
            OpacityProperty.OverrideMetadata(typeof(FontAwesomeBlock), new UIPropertyMetadata(1.0, OnOpacityChanged));
        }

        public FontAwesomeBlock()
        {
            IsVisibleChanged += OnIsVisibleChanged;
        }

        public IFontAwesomeIcon Icon
        {
            get => (IFontAwesomeIcon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public IFontAwesomeIconStyle IconStyle
        {
            get => (IFontAwesomeIconStyle)GetValue(IconStyleProperty);
            set => SetValue(IconStyleProperty, value);
        }

        public bool Spin
        {
            get => (bool)GetValue(SpinProperty);
            set => SetValue(SpinProperty, value);
        }

        public double SpinDuration
        {
            get => (double)GetValue(SpinDurationProperty);
            set => SetValue(SpinDurationProperty, value);
        }

        public bool ReverseSpinDirection
        {
            get => (bool)GetValue(ReverseSpinDirectionProperty);
            set => SetValue(ReverseSpinDirectionProperty, value);
        }

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }
        
        public FlipOrientation FlipOrientation
        {
            get => (FlipOrientation)GetValue(FlipOrientationProperty);
            set => SetValue(FlipOrientationProperty, value);
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FontAwesomeBlock)?.RefreshIconData();
        }

        private static void OnIconStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FontAwesomeBlock)?.RefreshIconData();
        }

        private static void OnSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeBlock fontAwesome)) 
                return;

            if ((bool)e.NewValue)
                fontAwesome.BeginSpin();
            else
            {
                fontAwesome.StopSpin();
                fontAwesome.SetRotation();
            }
        }

        private static void OnSpinDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(SpinProperty);

            if (!(d is FontAwesomeBlock fontAwesome) || !fontAwesome.Spin ||
                !(e.NewValue is double) || e.NewValue.Equals(e.OldValue))
                return;

            fontAwesome.StopSpin();
            fontAwesome.BeginSpin();
        }

        private static void OnReverseSpinDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeBlock fontAwesome) || !fontAwesome.Spin ||
                !(e.NewValue is bool) || e.NewValue.Equals(e.OldValue))
                return;

            fontAwesome.StopSpin();
            fontAwesome.BeginSpin();
        }

        private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeBlock fontAwesome) || fontAwesome.Spin || 
                !(e.NewValue is double) || e.NewValue.Equals(e.OldValue)) 
                return;

            fontAwesome.SetRotation();
        }

        private static void OnFlipOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeBlock fontAwesome) ||
                !(e.NewValue is FlipOrientation) || e.NewValue.Equals(e.OldValue)) 
                return;

            fontAwesome.SetFlipOrientation();
        }

        private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(SpinProperty);
        }
        
        private void OnIsVisibleChanged(object s, DependencyPropertyChangedEventArgs a)
        {
            CoerceValue(SpinProperty);
        }
        
        private static object SpinCoerceValue(DependencyObject d, object basevalue)
        {
            if (!(d is FontAwesomeBlock fontAwesome))
                return false;

            if (!fontAwesome.IsVisible || fontAwesome.Opacity == 0.0 || fontAwesome.SpinDuration == 0.0)
                return false;

            return basevalue;
        }

        private static object SpinDurationCoerceValue(DependencyObject d, object value)
        {
            var val = (double)value;
            return val < 0 ? 0d : value;
        }

        private static object RotationCoerceValue(DependencyObject d, object value)
        {
            var val = (double)value;
            return val % 360;
        }

        private void RefreshIconData()
        {
            if (Icon == null ||
                Icon.Styles == null || Icon.Styles.Length == 0)
            {
                SetValue(TextProperty, string.Empty);
                ClearValue(FontFamilyProperty);
                ClearValue(TextAlignmentProperty);
                return;
            }

            var styles = Icon.Styles;
            var style = IconStyle;

            if (!styles.Contains(style))
                style = styles.FirstOrDefault();

            SetValue(FontFamilyProperty, style.GetFontFamily());
            SetValue(TextAlignmentProperty, TextAlignment.Center);
            SetValue(TextProperty, char.ConvertFromUtf32((int)Icon.Id));
        }
    }
}
