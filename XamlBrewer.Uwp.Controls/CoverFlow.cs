namespace XamlBrewer.Uwp.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Windows.Foundation;
    using Windows.System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;

    public delegate void SelectedItemChangedEvent(CoverFlowEventArgs e);

    public class CoverFlow : ListBox
    {
        private Orientation orientation = Orientation.Horizontal;
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        private List<ListBoxItem> items;
        private List<ListBoxItem> tempItems;
        private double k { get { return SpaceBetweenSelectedItemAndItems; } }
        private double l { get { return SpaceBetweenItems; } }
        private double r { get { return RotationAngle; } }
        private double z { get { return ZDistance; } }
        private double s { get { return Scale; } }
        private int si;
        private int pageCount = 0;

        private Point itemRemderTransformOrigin = new Point(.5, .5);

        private double manipulationDistance = 0.0;
        private bool isManipulationActive = true;

        public Point ItemRenderTransformOrigin
        {
            get
            {
                return itemRemderTransformOrigin;
            }
            set
            {
                itemRemderTransformOrigin = value;
                foreach (ListBoxItem item in items)
                    item.RenderTransformOrigin = itemRemderTransformOrigin;
            }
        }

        #region Dependency Properties

        public double ASI
        {
            get
            {
                return (double)GetValue(ASIProperty);
            }
            set
            {
                SetValue(ASIProperty, value);
            }
        }

        public static readonly DependencyProperty ASIProperty =
            DependencyProperty.Register("ASIProperty", typeof(double), typeof(CoverFlow), new PropertyMetadata(0d, new PropertyChangedCallback(CoverFlow.OnASIChanged)));

        private static void OnASIChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoverFlow c = d as CoverFlow;
            c.SetItemPositions();
        }

        public double SpaceBetweenItems
        {
            get { return (double)GetValue(SpaceBetweenItemsProperty); }
            set { SetValue(SpaceBetweenItemsProperty, value); }
        }

        public static readonly DependencyProperty SpaceBetweenItemsProperty =
            DependencyProperty.Register("SpaceBetweenItems", typeof(double), typeof(CoverFlow), new PropertyMetadata(100d, new PropertyChangedCallback(CoverFlow.OnValuesChanged)));

        public double SpaceBetweenSelectedItemAndItems
        {
            get { return (double)GetValue(SpaceBetweenSelectedItemAndItemsProperty); }
            set { SetValue(SpaceBetweenSelectedItemAndItemsProperty, value); }
        }

        public static readonly DependencyProperty SpaceBetweenSelectedItemAndItemsProperty =
            DependencyProperty.Register("SpaceBetweenSelectedItemAndItems", typeof(double), typeof(CoverFlow), new PropertyMetadata(250d, new PropertyChangedCallback(CoverFlow.OnValuesChanged)));

        public double RotationAngle
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }

        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.Register("RotationAngle", typeof(double), typeof(CoverFlow), new PropertyMetadata(45d, new PropertyChangedCallback(CoverFlow.OnValuesChanged)));

        public double ZDistance
        {
            get { return (double)GetValue(ZDistanceProperty); }
            set { SetValue(ZDistanceProperty, value); }
        }

        public static readonly DependencyProperty ZDistanceProperty =
            DependencyProperty.Register("ZDistance", typeof(double), typeof(CoverFlow), new PropertyMetadata(0.0d, new PropertyChangedCallback(CoverFlow.OnValuesChanged)));

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(CoverFlow), new PropertyMetadata(.7d, new PropertyChangedCallback(CoverFlow.OnValuesChanged)));

        public static readonly DependencyProperty ManipulationThresholdProperty =
            DependencyProperty.Register("ManipulationThreshold", typeof(Double), typeof(CoverFlow), new PropertyMetadata(80d));


        public Double ManipulationThreshold
        {
            get { return (Double)GetValue(ManipulationThresholdProperty); }
            set { SetValue(ManipulationThresholdProperty, value); }
        }

        private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CoverFlow).SetItemPositions();
        }

        #endregion

        public CoverFlow()
        {
            DefaultStyleKey = typeof(CoverFlow);
            items = new List<ListBoxItem>();
            tempItems = new List<ListBoxItem>();

            SizeChanged += CoverFlowControl_SizeChanged;
            SelectionChanged += CoverFlowControl_SelectionChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateRailsX;
            this.ManipulationStarted += OnManipulationStarted;
            this.ManipulationDelta += OnManipulationDelta;

            this.SelectionChanged += CoverFlow_SelectionChanged;
        }

        private void CoverFlow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Selection changed to " + this.SelectedIndex);

        }

        #region Touch, Mouse, and Keyboard handling

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.MouseWheelDelta > 0 && this.SelectedIndex > 0)
                this.SelectedIndex--;
            else if (e.GetCurrentPoint(null).Properties.MouseWheelDelta < 0 && this.SelectedIndex < Items.Count - 1)
                this.SelectedIndex++;

            base.OnPointerWheelChanged(e);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            // Arrows and US keyboard.

            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.D)
            {
                //NextItem();
                e.Handled = true;
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.A)
            {
                //PreviousItem();
                e.Handled = true;
            }
            else if (e.Key == VirtualKey.PageDown || e.Key == VirtualKey.S || e.Key == VirtualKey.Down)
            {
                //NextPage();
                e.Handled = true;
            }
            else if (e.Key == VirtualKey.PageUp || e.Key == VirtualKey.W || e.Key == VirtualKey.Up)
            {
                //PreviousPage();
                e.Handled = true;
            }
            else if (e.Key == VirtualKey.Home || e.Key == VirtualKey.Q)
            {
                First();
                e.Handled = true;
            }
            else if (e.Key == VirtualKey.End || e.Key == VirtualKey.E)
            {
                Last();
                e.Handled = true;
            }
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            manipulationDistance = 0.0;
            isManipulationActive = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            manipulationDistance += e.Delta.Translation.X;

            if (isManipulationActive || manipulationDistance < -ManipulationThreshold || manipulationDistance > ManipulationThreshold)
            {
                manipulationDistance = 0.0;
                isManipulationActive = false;

                // TODO: find a way to give focus to the control, so that keyboard manipulation is restored.

                if (e.Delta.Translation.X < 0 && this.SelectedIndex < Items.Count - 1)
                    this.SelectedIndex++;
                else if (e.Delta.Translation.X > 0 && this.SelectedIndex > 0)
                    this.SelectedIndex--;
            }
        }

        #endregion

        public void SetItemPositions()
        {
            foreach (ListBoxItem item in tempItems)
            {
                item.Visibility = Visibility.Collapsed;
            }

            tempItems.Clear();

            if (items.Count == 0)
                return;

            si = (int)Math.Round(ASI, 0);
            if (si < 0)
                return;
            for (int x = 0; x <= pageCount + 1; x++)
            {
                if (x == 0)
                {
                    SetLocation(items[si], si);
                    continue;
                }
                if (si - x > -1)
                    SetLocation(items[si - x], si - x);
                if (si + x < items.Count)
                    SetLocation(items[si + x], si + x);
            }
        }

        private void SetLocation(ListBoxItem item, int index)
        {
            item.Visibility = Visibility.Visible;

            tempItems.Add(item);

            double t = -(ASI - index);
            double tk = k;
            double tr = r;
            double ts = s;
            double tz = z;
            int d = index - si;

            if (t < 1 && t > -1)
            {
                tk *= t;
                tr *= t;
                double tab = Math.Abs(t);
                ts += (1 - s) * (1 - tab);
                tz *= tab;
            }
            else if (t > 0)
            {
                tk += l * (t - 1);
            }
            else if (t < 0)
            {
                tk = -tk + (l * (t + 1));
                tr = -tr;
                d = si - index;
            }

            ((ScaleTransform)((TransformGroup)item.RenderTransform).Children[0]).ScaleX = ts;
            ((ScaleTransform)((TransformGroup)item.RenderTransform).Children[0]).ScaleY = ts;

            if (Orientation == Orientation.Horizontal)
                ((TranslateTransform)((TransformGroup)item.RenderTransform).Children[1]).X = tk;
            else
                ((TranslateTransform)((TransformGroup)item.RenderTransform).Children[1]).Y = tk;

            if (Orientation == Orientation.Horizontal)
                ((PlaneProjection)item.Projection).RotationY = tr;
            else
                ((PlaneProjection)item.Projection).RotationX = tr;
            ((PlaneProjection)item.Projection).LocalOffsetZ = tz;

            Canvas.SetZIndex(item, -d);
        }

        void CoverFlowControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                ASI = SelectedIndex;
        }

        void CoverFlowControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry visibleArea = new RectangleGeometry();
            Rect clip = new Rect(0, 0, ActualWidth, ActualHeight);
            visibleArea.Rect = clip;
            Clip = visibleArea;

            if (Orientation == Orientation.Horizontal)
                pageCount = (int)Math.Ceiling(((ActualWidth / 2) - k) / l);
            else
                pageCount = (int)Math.Ceiling(((ActualHeight / 2) - k) / l);

            SetItemPositions();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            ListBoxItem item2 = element as ListBoxItem;
            if (item2 != null && !items.Contains(item2))
            {
                item2.Visibility = Visibility.Collapsed;

                if (Orientation == Orientation.Horizontal)
                    item2.HorizontalAlignment = HorizontalAlignment.Center;
                else
                    item2.VerticalAlignment = VerticalAlignment.Center;

                items.Add(item2);

                TransformGroup myTransformGroup = new TransformGroup();
                ScaleTransform scaleTransform = new ScaleTransform();
                TranslateTransform translateTransform = new TranslateTransform();
                PlaneProjection planeProjection = new PlaneProjection() { CenterOfRotationX = .5, CenterOfRotationY = .5, CenterOfRotationZ = .5 };

                myTransformGroup.Children.Add(scaleTransform);
                myTransformGroup.Children.Add(translateTransform);

                // Associate the transforms to the object 
                item2.RenderTransformOrigin = itemRemderTransformOrigin;
                item2.RenderTransform = myTransformGroup;
                item2.Projection = planeProjection;

                if (items.Count < pageCount + 1)
                {
                    SetLocation(item2, items.Count - 1);
                }
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            ListBoxItem item2 = element as ListBoxItem;
            items.Remove(item2);
        }

        public void Next()
        {
            if (SelectedIndex < Items.Count - 1)
                SelectedIndex++;
            else
                SelectedIndex = 0;
        }
        public void Prev()
        {
            if (SelectedIndex > 0)
                SelectedIndex--;
            else
                SelectedIndex = Items.Count - 1;
        }
        public void PageUp()
        {
            int temp = SelectedIndex - pageCount - 1;
            if (temp < 0)
                temp = 0;
            SelectedIndex = temp;
        }
        public void PageDown()
        {
            int temp = SelectedIndex + pageCount + 1;
            if (temp > Items.Count - 1)
                temp = Items.Count - 1;
            SelectedIndex = temp;
        }
        public void First()
        {
            SelectedIndex = 0;
        }
        public void Last()
        {
            SelectedIndex = Items.Count - 1;
        }
    }
}
