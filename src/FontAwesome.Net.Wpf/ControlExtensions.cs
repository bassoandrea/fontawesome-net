using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FontAwesome.Net.Wpf
{
    public static class ControlExtensions
    {
        private static string GetSpinnerStoryboardName<T>()
            where T : FrameworkElement, ISpinnable
        {
            return $@"{typeof(T)}SpinnerStoryboard";
        }

        public static void BeginSpin<T>(this T control)
            where T : FrameworkElement, ISpinnable
        {
            var transformGroup = control.RenderTransform as TransformGroup ?? new TransformGroup();

            var rotateTransform = transformGroup.Children.OfType<RotateTransform>().FirstOrDefault();

            var angle = 0D;
            if (rotateTransform != null)
            {
                angle = rotateTransform.Angle;
            }
            else
            {
                transformGroup.Children.Insert(0, new RotateTransform(angle));
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                From = control.ReverseSpinDirection ? (360 + angle) : angle,
                To = control.ReverseSpinDirection ?  angle : (360 + angle),
                AutoReverse = false,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(control.SpinDuration))
            };
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, control);
            Storyboard.SetTargetProperty(animation,
                new PropertyPath("(0).(1)[0].(2)", UIElement.RenderTransformProperty,
                    TransformGroup.ChildrenProperty, RotateTransform.AngleProperty));

            storyboard.Begin();
            control.Resources.Add(GetSpinnerStoryboardName<T>(), storyboard);
        }

        public static void StopSpin<T>(this T control)
            where T : FrameworkElement, ISpinnable
        {
            var storyboardName = GetSpinnerStoryboardName<T>();
            if (!(control.Resources[storyboardName] is Storyboard storyboard)) 
                return;

            storyboard.Stop();
            control.Resources.Remove(storyboardName);
        }

        public static void SetRotation<T>(this T control)
            where T : FrameworkElement, IRotatable
        {
            var transformGroup = control.RenderTransform as TransformGroup ?? new TransformGroup();

            var rotateTransform = transformGroup.Children.OfType<RotateTransform>().FirstOrDefault();

            if (rotateTransform != null)
            {
                rotateTransform.Angle = control.Rotation;
            }
            else
            {
                transformGroup.Children.Insert(0, new RotateTransform(control.Rotation));
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }

        public static void SetFlipOrientation<T>(this T control)
            where T : FrameworkElement, IFlippable
        {
            var transformGroup = control.RenderTransform as TransformGroup ?? new TransformGroup();

            var scaleX = control.FlipOrientation is FlipOrientation.None || control.FlipOrientation is FlipOrientation.Vertical ? 1 : -1;
            var scaleY = control.FlipOrientation is FlipOrientation.None || control.FlipOrientation is FlipOrientation.Horizontal ? 1 : -1;

            var scaleTransform = transformGroup.Children.OfType<ScaleTransform>().FirstOrDefault();

            if (scaleTransform != null)
            {
                scaleTransform.ScaleX = scaleX;
                scaleTransform.ScaleY = scaleY;
            }
            else
            {
                transformGroup.Children.Add(new ScaleTransform(scaleX, scaleY));
                control.RenderTransform = transformGroup;
                control.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }
    }
}
