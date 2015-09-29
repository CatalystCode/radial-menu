﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMenuControl.UserControl
{
    using System;
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;

    /// <summary>
    /// A Content Control that can be dragged around. Huge thanks to Diederik Kols for the smartness behind this.
    /// </summary>
    [TemplatePart(Name = BorderPartName, Type = typeof(Border))]
    public class Floating : ContentControl
    {
        private const string BorderPartName = "DraggingBorder";

        public static readonly DependencyProperty IsBoundByParentProperty =
            DependencyProperty.Register("IsBoundByParent", typeof(bool), typeof(Floating), new PropertyMetadata(false));

        public static readonly DependencyProperty IsBoundByScreenProperty =
            DependencyProperty.Register("IsBoundByScreen", typeof(bool), typeof(Floating), new PropertyMetadata(false));

        private Border border;

        /// <summary>
        /// Initializes a new instance of the <see cref="Floating"/> class.
        /// </summary>
        public Floating()
        {
            this.DefaultStyleKey = typeof(Floating);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is bound by its parent size.
        /// </summary>
        public bool IsBoundByParent
        {
            get { return (bool)GetValue(IsBoundByParentProperty); }
            set { SetValue(IsBoundByParentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is bound by the screen size.
        /// </summary>
        public bool IsBoundByScreen
        {
            get { return (bool)GetValue(IsBoundByScreenProperty); }
            set { SetValue(IsBoundByScreenProperty, value); }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate.
        /// In simplest terms, this means the method is called just before a UI element displays in your app.
        /// Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            // Border
            this.border = this.GetTemplateChild(BorderPartName) as Border;
            if (this.border != null)
            {
                // this.border.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.TranslateInertia;
                this.border.ManipulationDelta += this.Border_ManipulationDelta;

                // Move Canvas properties from control to border.
                Canvas.SetLeft(this.border, Canvas.GetLeft(this));
                Canvas.SetLeft(this, 0);
                Canvas.SetTop(this.border, Canvas.GetTop(this));
                Canvas.SetTop(this, 0);

                // Move Margin to border.
                this.border.Padding = this.Margin;
                this.Margin = new Thickness(0);
            }
            else
            {
                // Exception
                throw new Exception("Floating Control Style has no Border.");
            }

            this.Loaded += Floating_Loaded;
        }

        private void Floating_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement el = GetClosestParentWithSize(this);
            if (el == null)
            {
                return;
            }

            el.SizeChanged += Floating_SizeChanged;
        }

        private void Floating_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var left = Canvas.GetLeft(this.border);
            var top = Canvas.GetTop(this.border);

            Rect rect = new Rect(left, top, this.border.ActualWidth, this.border.ActualHeight);

            AdjustCanvasPosition(rect);
        }

        private void Border_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            ManipulateControlPosition(e.Delta.Translation.X, e.Delta.Translation.Y);
        }

        /// <summary>
        /// Manipulate the control's positon!
        /// </summary>
        /// <param name="X">Delta on the X axis</param>
        /// <param name="Y">Delta on the Y axis</param>
        public void ManipulateControlPosition(double X, double Y)
        {
            var left = Canvas.GetLeft(this.border) + X;
            var top = Canvas.GetTop(this.border) + Y;

            Rect rect = new Rect(left, top, this.border.ActualWidth, this.border.ActualHeight);
            AdjustCanvasPosition(rect);
        }

        /// <summary>
        /// Adjusts the canvas position according to the IsBoundBy* properties.
        /// </summary>
        private void AdjustCanvasPosition(Rect rect)
        {
            // No boundaries
            if (!this.IsBoundByParent && !this.IsBoundByScreen)
            {
                Canvas.SetLeft(this.border, rect.Left);
                Canvas.SetTop(this.border, rect.Top);

                return;
            }

            FrameworkElement el = GetClosestParentWithSize(this);

            // No parent
            if (el == null)
            {
                // We probably never get here.
                return;
            }

            var position = new Point(rect.Left, rect.Top); ;

            if (this.IsBoundByParent)
            {
                Rect parentRect = new Rect(0, 0, el.ActualWidth, el.ActualHeight);
                position = AdjustedPosition(rect, parentRect);
            }

            if (this.IsBoundByScreen)
            {
                var ttv = el.TransformToVisual(Window.Current.Content);
                var topLeft = ttv.TransformPoint(new Point(0, 0));
                Rect parentRect = new Rect(topLeft.X, topLeft.Y, Window.Current.Bounds.Width - topLeft.X, Window.Current.Bounds.Height - topLeft.Y);
                position = AdjustedPosition(rect, parentRect);
            }

            // Set new position
            Canvas.SetLeft(this.border, position.X);
            Canvas.SetTop(this.border, position.Y);
        }

        /// <summary>
        /// Returns the adjusted the topleft position of a rectangle so that is stays within a parent rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="parentRect">The parent rectangle.</param>
        /// <returns></returns>
        private Point AdjustedPosition(Rect rect, Rect parentRect)
        {
            var left = rect.Left;
            var top = rect.Top;

            if (left < -parentRect.Left)
            {
                left = -parentRect.Left;
            }
            else if (left + rect.Width > parentRect.Width)
            {
                left = parentRect.Width - rect.Width;
            }

            if (top < -parentRect.Top)
            {
                top = -parentRect.Top;
            }
            else if (top + rect.Height > parentRect.Height)
            {
                top = parentRect.Height - rect.Height;
            }

            return new Point(left, top);
        }

        /// <summary>
        /// Gets the closest parent with a real size.
        /// </summary>
        private FrameworkElement GetClosestParentWithSize(FrameworkElement element)
        {
            while (element != null && (element.ActualHeight == 0 || element.ActualWidth == 0))
            {
                element = element.Parent as FrameworkElement;
            }

            return element;
        }
    }
}
