using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FontAwesome.Net.Wpf
{
    public class FontAwesomeImage
        : Image, ISpinnable, IRotatable, IFlippable
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(IFontAwesomeIcon), typeof(FontAwesomeImage), new PropertyMetadata(null, OnIconChanged));

        public static readonly DependencyProperty IconStyleProperty = DependencyProperty.Register(
            nameof(IconStyle), typeof(IFontAwesomeIconStyle), typeof(FontAwesomeImage), new PropertyMetadata(null, OnIconStyleChanged));

        public static readonly DependencyProperty SpinProperty = DependencyProperty.Register(
            nameof(Spin), typeof(bool), typeof(FontAwesomeImage), new PropertyMetadata(false, OnSpinChanged, SpinCoerceValue));

        public static readonly DependencyProperty SpinDurationProperty = DependencyProperty.Register(
            nameof(SpinDuration), typeof(double), typeof(FontAwesomeImage), new PropertyMetadata(1d, OnSpinDurationChanged, SpinDurationCoerceValue));

        public static readonly DependencyProperty ReverseSpinDirectionProperty = DependencyProperty.Register(
            nameof(ReverseSpinDirection), typeof(bool), typeof(FontAwesomeImage), new PropertyMetadata(false, OnReverseSpinDirectionChanged));

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(
            nameof(Rotation), typeof(double), typeof(FontAwesomeImage), new PropertyMetadata(0d, OnRotationChanged, RotationCoerceValue));

        public static readonly DependencyProperty FlipOrientationProperty = DependencyProperty.Register(
            nameof(FlipOrientation), typeof(FlipOrientation), typeof(FontAwesomeImage), new PropertyMetadata(FlipOrientation.None, OnFlipOrientationChanged));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            nameof(Foreground), typeof(Brush), typeof(FontAwesomeImage), new PropertyMetadata(Brushes.Black, OnForegroundChanged));

        static FontAwesomeImage()
        {
            OpacityProperty.OverrideMetadata(typeof(FontAwesomeImage), new UIPropertyMetadata(1.0, OnOpacityChanged));
        }

        public FontAwesomeImage()
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

        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FontAwesomeImage)?.RefreshIconData();
        }

        private static void OnIconStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FontAwesomeImage)?.RefreshIconData();
        }

        private static void OnSpinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeImage fontAwesome))
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

            if (!(d is FontAwesomeImage fontAwesome) || !fontAwesome.Spin ||
                !(e.NewValue is double) || e.NewValue.Equals(e.OldValue))
                return;

            fontAwesome.StopSpin();
            fontAwesome.BeginSpin();
        }

        private static void OnReverseSpinDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeImage fontAwesome) || !fontAwesome.Spin ||
                !(e.NewValue is bool) || e.NewValue.Equals(e.OldValue))
                return;

            fontAwesome.StopSpin();
            fontAwesome.BeginSpin();
        }

        private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeImage fontAwesome) || fontAwesome.Spin ||
                !(e.NewValue is double) || e.NewValue.Equals(e.OldValue))
                return;

            fontAwesome.SetRotation();
        }

        private static void OnFlipOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FontAwesomeImage fontAwesome) ||
                !(e.NewValue is FlipOrientation) || e.NewValue.Equals(e.OldValue))
                return;

            fontAwesome.SetFlipOrientation();
        }
        
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FontAwesomeImage)?.RefreshIconData();
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
            if (!(d is FontAwesomeImage fontAwesome))
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
                SetValue(SourceProperty, null);
                return;
            }

            var styles = Icon.Styles;
            var style = IconStyle;
            if (!styles.Contains(style))
                style = styles.FirstOrDefault();

            SetValue(SourceProperty, CreateImageSource(Icon, style, Foreground));
        }

        private static ImageSource CreateImageSource(IFontAwesomeIcon icon, IFontAwesomeIconStyle style, Brush foregroundBrush, double emSize = 100)
        {
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var temp = style.GetFontTypeface();

                drawingContext.DrawText(
                    new FormattedText(char.ConvertFromUtf32(icon.Id), CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight, temp, emSize, foregroundBrush)
                        { TextAlignment = TextAlignment.Center }, new Point(0, 0));
            }
            return new DrawingImage(visual.Drawing);
        }
    }
}
