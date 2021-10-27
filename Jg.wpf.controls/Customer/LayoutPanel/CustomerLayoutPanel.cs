using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Jg.wpf.controls.Behaviors;


namespace Jg.wpf.controls.Customer.LayoutPanel
{
    public class CustomerLayoutPanel : Panel
    {
        private const double DragScaleDefault = 1.0d;
        private const double NormalOpacity = 1.0d;
        private const double DragOpacityDefault = 0.6d;
        private const double OpacityMin = 0.1d;
        private const int ZIndexIntermediate = 1;
        private const int ZIndexDrag = 10;
        private static readonly TimeSpan DefaultAnimationTimeWithoutEasing = TimeSpan.FromMilliseconds(200);
        private static readonly TimeSpan DefaultAnimationTimeWithEasing = TimeSpan.FromMilliseconds(400);

        private readonly IList<UIElement> _fluidElements;
        private int _finalColumns;
        private int _finalRows;
        private Size _availableSize;

        private readonly CellsLayoutManager _layoutManager;
        private Point _dragStartPoint;
        private Point _lastMovePoint;
        private UIElement _dragElement;
        private UIElement _lastDragElement;
        private Vector _offset;
        private int _dragStartIndex;
        private int _internalVisibleChildrenCount;


        #region Routed Events and Dependency Properties

        /// <summary>
        /// event when a dragging item dropped
        /// </summary>
        public static readonly RoutedEvent ItemDroppedEvent = EventManager.RegisterRoutedEvent("ItemDropped",
                                                                             RoutingStrategy.Bubble,
                                                                             typeof(RoutedEventHandler),
                                                                             typeof(CustomerLayoutPanel));
        public event RoutedEventHandler ItemDropped
        {
            add => AddHandler(ItemDroppedEvent, value);
            remove => RemoveHandler(ItemDroppedEvent, value);
        }

        #region DragEasing

        /// <summary>
        /// DragEasing Dependency Property
        /// </summary>
        public static readonly DependencyProperty DragEasingProperty =
            DependencyProperty.Register("DragEasing", typeof(EasingFunctionBase), typeof(CustomerLayoutPanel),
                new FrameworkPropertyMetadata((OnDragEasingChanged)));

        /// <summary>
        /// Gets or sets the DragEasing property. This dependency property 
        /// indicates the Easing function to be used when the user stops dragging the child and releases it.
        /// </summary>
        public EasingFunctionBase DragEasing
        {
            get => (EasingFunctionBase)GetValue(DragEasingProperty);
            set => SetValue(DragEasingProperty, value);
        }

        /// <summary>
        /// Handles changes to the DragEasing property.
        /// </summary>
        /// <param name="d">CustomerLayoutPanel</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDragEasingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (CustomerLayoutPanel)d;
            var oldDragEasing = (EasingFunctionBase)e.OldValue;
            var newDragEasing = panel.DragEasing;
            panel.OnDragEasingChanged(oldDragEasing, newDragEasing);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DragEasing property.
        /// </summary>
        /// <param name="oldDragEasing">Old Value</param>
        /// <param name="newDragEasing">New Value</param>
        protected virtual void OnDragEasingChanged(EasingFunctionBase oldDragEasing, EasingFunctionBase newDragEasing)
        {

        }

        #endregion

        #region DragOpacity

        /// <summary>
        /// DragOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty DragOpacityProperty =
            DependencyProperty.Register("DragOpacity", typeof(double), typeof(CustomerLayoutPanel),
                new FrameworkPropertyMetadata(DragOpacityDefault,
                                              OnDragOpacityChanged,
                                              CoerceDragOpacity));

        /// <summary>
        /// Gets or sets the DragOpacity property. This dependency property 
        /// indicates the opacity of the child being dragged.
        /// </summary>
        public double DragOpacity
        {
            get { return (double)GetValue(DragOpacityProperty); }
            set { SetValue(DragOpacityProperty, value); }
        }


        /// <summary>
        /// Coerces the FluidDrag Opacity to an acceptable value
        /// </summary>
        /// <param name="d">Dependency Object</param>
        /// <param name="value">Value</param>
        /// <returns>Coerced Value</returns>
        private static object CoerceDragOpacity(DependencyObject d, object value)
        {
            double opacity = (double)value;

            if (opacity < OpacityMin)
            {
                opacity = OpacityMin;
            }
            else if (opacity > NormalOpacity)
            {
                opacity = NormalOpacity;
            }

            return opacity;
        }

        /// <summary>
        /// Handles changes to the DragOpacity property.
        /// </summary>
        /// <param name="d">CustomerLayoutPanel</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDragOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (CustomerLayoutPanel)d;
            var oldDragOpacity = (double)e.OldValue;
            var newDragOpacity = panel.DragOpacity;
            panel.OnDragOpacityChanged(oldDragOpacity, newDragOpacity);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DragOpacity property.
        /// </summary>
        /// <param name="oldDragOpacity">Old Value</param>
        /// <param name="newDragOpacity">New Value</param>
        protected virtual void OnDragOpacityChanged(double oldDragOpacity, double newDragOpacity)
        {

        }

        #endregion

        #region DragScale

        /// <summary>
        /// DragScale Dependency Property
        /// </summary>
        public static readonly DependencyProperty DragScaleProperty =
            DependencyProperty.Register("DragScale", typeof(double), typeof(CustomerLayoutPanel),
                new FrameworkPropertyMetadata(DragScaleDefault, OnDragScaleChanged));

        /// <summary>
        /// Gets or sets the DragScale property. This dependency property 
        /// indicates the factor by which the child should be scaled when it is dragged.
        /// </summary>
        public double DragScale
        {
            get => (double)GetValue(DragScaleProperty);
            set => SetValue(DragScaleProperty, value);
        }

        /// <summary>
        /// Handles changes to the DragScale property.
        /// </summary>
        /// <param name="d">CustomerLayoutPanel</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnDragScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (CustomerLayoutPanel)d;
            var oldDragScale = (double)e.OldValue;
            var newDragScale = panel.DragScale;
            panel.OnDragScaleChanged(oldDragScale, newDragScale);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DragScale property.
        /// </summary>
        /// <param name="oldDragScale">Old Value</param>
        /// <param name="newDragScale">New Value</param>
        protected virtual void OnDragScaleChanged(double oldDragScale, double newDragScale)
        {

        }

        #endregion

        #region ElementEasing

        /// <summary>
        /// ElementEasing Dependency Property
        /// </summary>
        public static readonly DependencyProperty ElementEasingProperty =
            DependencyProperty.Register("ElementEasing", typeof(EasingFunctionBase), typeof(CustomerLayoutPanel),
                new FrameworkPropertyMetadata((OnElementEasingChanged)));

        /// <summary>
        /// Gets or sets the ElementEasing property. This dependency property 
        /// indicates the Easing Function to be used when the elements are rearranged.
        /// </summary>
        public EasingFunctionBase ElementEasing
        {
            get => (EasingFunctionBase)GetValue(ElementEasingProperty);
            set => SetValue(ElementEasingProperty, value);
        }

        /// <summary>
        /// Handles changes to the ElementEasing property.
        /// </summary>
        /// <param name="d">CustomerLayoutPanel</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnElementEasingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (CustomerLayoutPanel)d;
            var oldElementEasing = (EasingFunctionBase)e.OldValue;
            var newElementEasing = panel.ElementEasing;
            panel.OnElementEasingChanged(oldElementEasing, newElementEasing);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ElementEasing property.
        /// </summary>
        /// <param name="oldElementEasing">Old Value</param>
        /// <param name="newElementEasing">New Value</param>
        /// 
        protected virtual void OnElementEasingChanged(EasingFunctionBase oldElementEasing, EasingFunctionBase newElementEasing)
        {

        }

        #endregion

        #region columns

        /// <summary>
        /// Columns Property
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(int), typeof(CustomerLayoutPanel),
                                                                    new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                                                     FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                                     FrameworkPropertyMetadataOptions.Inherits));


        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        #endregion

        #region Rows

        /// <summary>
        /// Rows Property
        /// </summary>
        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached("Rows", typeof(int), typeof(CustomerLayoutPanel),
                                                                    new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                                                     FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                                     FrameworkPropertyMetadataOptions.Inherits, OnRowsChanged));



        public int Rows
        {
            get => (int)GetValue(RowsProperty);
            set => SetValue(RowsProperty, value);
        }

        #endregion

        #region

        public static readonly DependencyProperty FreeItemHeightProperty =
            DependencyProperty.RegisterAttached("FreeItemHeight", typeof(bool), typeof(CustomerLayoutPanel),
                new FrameworkPropertyMetadata(true));

        public bool FreeItemHeight
        {
            get => (bool)GetValue(FreeItemHeightProperty);
            set => SetValue(FreeItemHeightProperty, value);
        }


        public bool FreeItemSize
        {
            get => (bool)GetValue(FreeItemSizeProperty);
            set => SetValue(FreeItemSizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for FreeItemSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FreeItemSizeProperty =
            DependencyProperty.Register("FreeItemSize", typeof(bool), typeof(CustomerLayoutPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool ReverseRow
        {
            get => (bool)GetValue(ReverseRowProperty);
            set => SetValue(ReverseRowProperty, value);
        }

        public static readonly DependencyProperty ReverseRowProperty =
            DependencyProperty.Register("ReverseRow", typeof(bool), typeof(CustomerLayoutPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange));


        public bool AllowPlaceHolder
        {
            get => (bool)GetValue(AllowPlaceHolderProperty);
            set => SetValue(AllowPlaceHolderProperty, value);
        }

        public static readonly DependencyProperty AllowPlaceHolderProperty =
            DependencyProperty.RegisterAttached("AllowPlaceHolder", typeof(bool), typeof(CustomerLayoutPanel),
                new FrameworkPropertyMetadata(true));

        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// 1.Calculate cell width, rows by available width, internal children and Columns
        /// 2.Get desired height as cell height
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns>Size(cellWith * columns, cellHeight * rows)</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateInternalVisibleChildrenCount();
            if (FreeItemSize)
            {
                UpdateComputedValues();
                _fluidElements.Clear();
                var itemAvailableSize = new Size(availableSize.Width / _finalColumns, availableSize.Height / _finalRows);
                var itemWidth = 0.0;
                var itemHeight = 0.0;
                var index = 0;
                for (var count = InternalChildren.Count; index < count; ++index)
                {
                    var child = InternalChildren[index];
                    child.Measure(itemAvailableSize);
                    var desiredSize = child.DesiredSize;
                    if (itemWidth < desiredSize.Width)
                        itemWidth = desiredSize.Width;
                    if (itemHeight < desiredSize.Height)
                        itemHeight = desiredSize.Height;
                    _fluidElements.Add(child);
                }
                return new Size(itemWidth * _finalColumns, itemHeight * _finalRows);
            }
            else
            {
                var totalColumnWidth = 0.0;
                var totalRowHeight = 0.0;

                var childrenCount = _internalVisibleChildrenCount;
                if (childrenCount > 0)
                {
                    _fluidElements.Clear();

                    if (Columns > 0)
                    {
                        _finalColumns = Columns;
                        if (Rows <= 0)
                        {
                            _finalRows = childrenCount % Columns == 0
                                       ? childrenCount / Columns
                                       : childrenCount / Columns + 1;
                        }
                        else
                        {
                            _finalRows = Rows;
                        }
                    }
                    else
                    {
                        if (Rows > 0)
                        {
                            _finalRows = Rows;
                            _finalColumns = childrenCount % Rows == 0
                                       ? childrenCount / _finalRows
                                       : childrenCount / _finalRows + 1;
                        }
                    }
                    var itemWidth = double.IsPositiveInfinity(availableSize.Width) ? double.PositiveInfinity : availableSize.Width / _finalColumns;
                    var itemHeight = double.IsPositiveInfinity(availableSize.Height) ? double.PositiveInfinity : availableSize.Height / _finalRows;

                    // Iterate through all the UIElements in the Children collection
                    for (var i = 0; i < childrenCount; i++)
                    {
                        var child = InternalChildren[i];

                        if (child != null)
                        {
                            var availableItemSize = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
                            // Ask the child how much size it needs
                            child.Measure(availableItemSize);

                            //todo:When the height of item is binding to other control values, the child.Measure return value is incorrect
                            if (FreeItemHeight)
                            {
                                if (itemHeight <= 0 || double.IsPositiveInfinity(itemHeight) || Math.Abs(itemHeight - child.DesiredSize.Height) > 0)
                                {
                                    itemHeight = child.DesiredSize.Height;
                                }

                                itemWidth = child.DesiredSize.Width;
                            }
                            else
                            {
                                if (itemHeight <= 0 || double.IsPositiveInfinity(itemHeight))
                                {
                                    itemHeight = child.DesiredSize.Height;
                                }
                                if (itemWidth <= 0 || double.IsPositiveInfinity(itemWidth))
                                {
                                    itemWidth = child.DesiredSize.Width;
                                }

                                if (child is FrameworkElement frameworkElement)
                                {
                                    frameworkElement.Width = itemWidth;
                                    frameworkElement.Height = itemHeight;
                                }
                            }

                            // Check if the child is already added to the fluidElements collection
                            if (!_fluidElements.Contains(child))
                            {
                                _fluidElements.Add(child);
                            }
                        }
                    }

                    totalColumnWidth = itemWidth * _finalColumns;
                    totalRowHeight = itemHeight * _finalRows;
                    if (totalColumnWidth >= 0 && totalRowHeight >= 0)
                    {
                        _availableSize = new Size(totalColumnWidth, totalRowHeight);
                    }
                }

                return new Size(totalColumnWidth, totalRowHeight);
            }
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (FreeItemSize)
            {
                var finalRect = new Rect(0.0, 0.0, finalSize.Width / (double)_finalColumns, finalSize.Height / (double)_finalRows);

                if (Children.Count > 0 && _fluidElements.Count > 0)
                {
                    // Initialize the LayoutManager
                    var orientation = Orientation.Horizontal;
                    if (Rows > 0 && Columns <= 0)
                    {
                        orientation = Orientation.Vertical;
                    }

                    _layoutManager.Initialize(finalSize, _finalColumns, _finalRows, orientation, ReverseRow);

                    // Update the Layout
                    for (var index = 0; index < _fluidElements.Count; index++)
                    {
                        UIElement element = _fluidElements[index];
                        if (element == null)
                            continue;

                        // If an child is currently being dragged, then no need to animate it
                        if (_dragElement != null && index == _fluidElements.IndexOf(_dragElement))
                            continue;

                        var pos = _layoutManager.GetPointFromIndex(index);
                        element.RenderTransform = _layoutManager.CreateTransform(pos.X, pos.Y, 1.0, 1.0);
                        // Get the cell position of the current index                          
                        //element.Arrange(new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));
                        element.Arrange(finalRect);
                    }
                }

            }
            else
            {
                if (Children.Count > 0 && _fluidElements.Count > 0)
                {
                    // Initialize the LayoutManager
                    var orientation = Orientation.Horizontal;
                    if (Rows > 0 && Columns <= 0)
                    {
                        orientation = Orientation.Vertical;
                    }

                    _layoutManager.Initialize(_availableSize, _finalColumns, _finalRows, orientation, ReverseRow);

                    // Update the Layout
                    InitializeFluidLayout();
                }
            }
            return finalSize;
        }
        private void UpdateComputedValues()
        {
            var childrenCount = _internalVisibleChildrenCount;
            if (childrenCount == 0)
            {
                childrenCount = 1;
            }

            if (Columns > 0)
            {
                _finalColumns = Columns;
                if (Rows <= 0)
                {
                    _finalRows = (childrenCount + (_finalColumns - 1)) / _finalColumns;
                }
                else
                {
                    _finalRows = Rows;
                }
            }
            else
            {
                if (Rows <= 0)
                {
                    _finalRows = (int)Math.Sqrt((double)_finalColumns);
                    if (_finalRows * _finalRows < _finalColumns)
                    {
                        ++_finalRows;
                    }

                    _finalColumns = _finalRows;
                }
                else
                {
                    _finalRows = Rows;
                    _finalColumns = (childrenCount + (_finalRows - 1)) / _finalRows;
                }
            }
        }

        private void UpdateInternalVisibleChildrenCount()
        {
            _internalVisibleChildrenCount = 0;
            if (AllowPlaceHolder)
            {
                _internalVisibleChildrenCount = InternalChildren.Count;
            }
            else
            {
                foreach (var child in InternalChildren)
                {
                    if (child is UIElement element && element.Visibility == Visibility.Visible)
                    {
                        _internalVisibleChildrenCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Initialize LayoutManager and locate child items
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>

        #endregion

        #region Construction

        public CustomerLayoutPanel()
        {
            _fluidElements = new List<UIElement>();
            _layoutManager = new CellsLayoutManager();
            Loaded += OnCustomLoaded;
        }

        private void OnCustomLoaded(object sender, RoutedEventArgs e)
        {
            //InitializeFluidLayout();
        }

        #endregion

        #region Helpers

        public void InitializeFluidLayout()
        {
            // Iterate through all the fluid elements and animate their
            // movement to their new location.
            for (int index = 0; index < _fluidElements.Count; index++)
            {
                var element = _fluidElements[index];
                if (element == null)
                    continue;

                var availableItemSize = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
                // Ask the child how much size it needs
                element.Measure(availableItemSize);

                // If an child is currently being dragged, then no need to animate it
                if (_dragElement != null && index == _fluidElements.IndexOf(_dragElement))
                    continue;

                var pos = _layoutManager.GetPointFromIndex(index);
                //Logger.WriteLineInfo("Point index:{2} - pos x:{0}, y:{1}", pos.X, pos.Y, index);
                element.RenderTransform = _layoutManager.CreateTransform(pos.X, pos.Y, 1.0, 1.0);
                // Get the cell position of the current index                          
                element.Arrange(new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));
            }
        }

        /// <summary>
        /// Iterates through all the fluid elements and animate their
        /// movement to their new location.
        /// </summary>
        private void UpdateFluidLayout()
        {
            // Iterate through all the fluid elements and animate their
            // movement to their new location.
            for (var index = 0; index < _fluidElements.Count; index++)
            {
                UIElement element = _fluidElements[index];
                if (element == null)
                    continue;

                // If an child is currently being dragged, then no need to animate it
                if (_dragElement != null && index == _fluidElements.IndexOf(_dragElement))
                    continue;

                element.Arrange(FreeItemSize
                    ? new Rect(0, 0, _layoutManager.CellSize.Width, _layoutManager.CellSize.Height)
                    : new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));

                // Get the cell position of the current index
                var pos = _layoutManager.GetPointFromIndex(index);

                Storyboard transition;
                // Is the child being animated the same as the child which was last dragged?
                if (element == _lastDragElement)
                {
                    // Is easing function specified for the animation?
                    var duration = (DragEasing != null) ? DefaultAnimationTimeWithEasing : DefaultAnimationTimeWithoutEasing;
                    // Create the Storyboard for the transition
                    transition = _layoutManager.CreateTransition(element, pos, duration, DragEasing);

                    // When the user releases the drag child, it's Z-Index is set to 1 so that 
                    // during the animation it does not go below other elements.
                    // After the animation has completed set its Z-Index to 0
                    transition.Completed += (s, e) =>
                    {
                        if (_lastDragElement != null)
                        {
                            _lastDragElement.SetValue(ZIndexProperty, 0);
                            _lastDragElement = null;
                        }
                    };
                }
                else // It is a non-dragElement
                {
                    // Is easing function specified for the animation?
                    var duration = (ElementEasing != null) ? DefaultAnimationTimeWithEasing : DefaultAnimationTimeWithoutEasing;
                    // Create the Storyboard for the transition
                    transition = _layoutManager.CreateTransition(element, pos, duration, ElementEasing);
                }

                // Start the animation                
                transition.Begin();
            }
        }

        /// <summary>
        /// Moves the dragElement to the new Index
        /// </summary>
        /// <param name="newIndex">Index of the new location</param>
        /// <returns>True-if dragElement was moved otherwise False</returns>
        private bool UpdateDragElementIndex(int newIndex)
        {
            // Check if the dragElement is being moved to its current place
            // If yes, then no need to proceed further. (Improves efficiency!)
            var dragCellIndex = _fluidElements.IndexOf(_dragElement);
            if (dragCellIndex == newIndex)
                return false;

            _fluidElements.RemoveAt(dragCellIndex);
            _fluidElements.Insert(newIndex, _dragElement);

            return true;
        }

        #endregion

        #region Drag drop methods

        /// <summary>
        /// Handler for the event when the user starts dragging the dragElement.
        /// </summary>
        /// <param name="child">UIElement being dragged</param>
        /// <param name="position">Position in the child where the user clicked</param>
        public void BeginDrag(UIElement child, Point position)
        {
            switch (child)
            {
                case null:
                    return;
                case FrameworkElement element:
                    _offset = VisualTreeHelper.GetOffset(element);
                    break;
            }

            // Call the event handler core on the Dispatcher. (Improves efficiency!)
            Dispatcher.BeginInvoke(new Action(() =>
            {
                child.Opacity = DragOpacity;
                child.SetValue(ZIndexProperty, ZIndexDrag);
                // Capture further mouse events
                child.CaptureMouse();
                _dragElement = child;
                _lastDragElement = null;

                // Since we are scaling the dragElement by DragScale, the clickPoint also shifts
                _dragStartPoint = new Point(position.X * DragScale, position.Y * DragScale);
                _dragStartIndex = _fluidElements.IndexOf(child);
            }));
        }

        /// <summary>
        /// Handler for the event when the user drags the dragElement.
        /// </summary>
        /// <param name="child">UIElement being dragged</param>
        /// <param name="position">Position where the user clicked w.r.t. the UIElement being dragged</param>
        /// <param name="positionInParent">Position where the user clicked w.r.t. the CustomerLayoutPanel (the parentFWPanel of the UIElement being dragged</param>
        /// <param name="associatedObject">Associated Object</param>
        public void DragMove(UIElement child, Point position, Point positionInParent, ItemsControl associatedObject)
        {
            if (child == null || positionInParent == _lastMovePoint)
            {
                return;
            }

            // Call the event handler core on the Dispatcher. (Improves efficiency!)
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var transX = 0d;
                var transY = 0d;

                switch (DragOrientation)
                {
                    case DragOrientation.Horizontal:
                        transX = positionInParent.X - _dragStartPoint.X - _offset.X;
                        break;
                    case DragOrientation.Vertical:
                        transY = positionInParent.Y - _dragStartPoint.Y - _offset.Y;
                        break;
                    case DragOrientation.All:
                        transX = positionInParent.X - _dragStartPoint.X - _offset.X;
                        transY = positionInParent.Y - _dragStartPoint.Y - _offset.Y;
                        break;
                }

                if (_dragElement != null && _layoutManager != null)
                {

                    _dragElement.RenderTransform = _layoutManager.CreateTransform(transX, transY, DragScale, DragScale);

                    // Get the index in the fluidElements list corresponding to the current mouse location
                    var currentPt = positionInParent;
                    var index = _layoutManager.GetIndexFromPoint(currentPt);

                    // If no valid cell index is obtained, add the child to the end of the 
                    // fluidElements list.
                    if ((index == -1) || (index >= _fluidElements.Count))
                    {
                        index = _fluidElements.Count - 1;
                    }
                    if (associatedObject is ListBox listBox)
                    {
                        if (index > 1 && index < listBox.Items.Count - 1)
                        {
                            listBox.ScrollIntoView(listBox.Items[index]);
                        }
                    }
                    if (associatedObject is DataGrid grid)
                    {
                        if (index > 1 && index < grid.Items.Count - 1)
                        {
                            grid.ScrollIntoView(grid.Items[index]);
                        }
                    }
                    // If the dragElement is moved to a new location, then only
                    // call the updation of the layout.
                    if (UpdateDragElementIndex(index))
                    {
                        UpdateFluidLayout();
                    }
                    _lastMovePoint = positionInParent;
                }
            }));
        }

        /// <summary>
        /// Handler for the event when the user stops dragging the dragElement and releases it.
        /// </summary>
        /// <param name="child">UIElement being dragged</param>
        /// <param name="position">Position where the user clicked w.r.t. the UIElement being dragged</param>
        /// <param name="positionInParent">Position where the user clicked w.r.t. the CustomerLayoutPanel (the parentFWPanel of the UIElement being dragged</param>
        public void EndDrag(UIElement child, Point position, Point positionInParent)
        {
            if (child == null)
                return;

            // Call the event handler core on the Dispatcher. (Improves efficiency!)
            Dispatcher.Invoke(() =>
            {
                var transX = 0d;
                var transY = 0d;

                switch (DragOrientation)
                {
                    case DragOrientation.Horizontal:
                        transX = positionInParent.X - _dragStartPoint.X - _offset.X;
                        break;
                    case DragOrientation.Vertical:
                        transY = positionInParent.Y - _dragStartPoint.Y - _offset.Y;
                        break;
                    case DragOrientation.All:
                        transX = positionInParent.X - _dragStartPoint.X - _offset.X;
                        transY = positionInParent.Y - _dragStartPoint.Y - _offset.Y;
                        break;
                }

                if ((_dragElement != null) && (_layoutManager != null))
                {
                    _dragElement.RenderTransform = _layoutManager.CreateTransform(transX, transY, DragScale, DragScale);

                    child.Opacity = NormalOpacity;
                    // Z-Index is set to 1 so that during the animation it does not go below other elements.
                    child.SetValue(ZIndexProperty, ZIndexIntermediate);
                    // Release the mouse capture
                    child.ReleaseMouseCapture();

                    // Reference used to set the Z-Index to 0 during the UpdateFluidLayout
                    _lastDragElement = _dragElement;

                    _dragElement = null;
                    UpdateFluidLayout();

                    RaiseItemDroppedEvent(_dragStartIndex, _fluidElements.IndexOf(_lastDragElement), ((FrameworkElement)_lastDragElement).DataContext); //Rasie drop event                    
                }
            });
        }

        private void RaiseItemDroppedEvent(int previousIndex, int currentIndex, object dataContext)
        {
            RoutedEventArgs newEventArgs = new ItemDroppedEventArgs(ItemDroppedEvent, previousIndex, currentIndex, dataContext);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region layout manager

        /// <summary>
        /// Manager cells' positions
        /// </summary>
        private sealed class CellsLayoutManager
        {
            #region Fields

            private Size _panelSize;
            private Size _cellSize;
            private Orientation _panelOrientation;
            private int _cellsPerLine;
            private bool _reverseRow;
            private int _finalRows;
            public Size CellSize => _cellSize;

            #endregion

            #region Methods

            /// <summary>
            /// Initializes properties
            /// </summary>
            /// <param name="panelSize">With and height of Panel</param>       
            /// <param name="columns">Width of each child in the Panel</param>
            /// <param name="rows">Height of each child in the Panel</param>
            /// <param name="orientation">Orientation of the panel - Horizontal or Vertical
            /// Horizontal - cells number of one row is fixed
            /// Verical - cells number of on column is fixed
            /// </param>
            /// <param name="reverseRow">reverse row</param>        
            internal void Initialize(Size panelSize, int columns, int rows, Orientation orientation, bool reverseRow)
            {
                if ((panelSize.Width <= 0.0d) || (panelSize.Height <= 0.0d))
                {
                    _cellsPerLine = 0;
                    return;
                }

                _panelSize = panelSize;
                _cellSize = new Size(panelSize.Width / columns, panelSize.Height / rows);
                _panelOrientation = orientation;

                _cellsPerLine = orientation == Orientation.Horizontal ? columns : rows;
                _reverseRow = reverseRow;
                _finalRows = rows;
            }

            /// <summary>
            /// Provides the index of the child (in the CustomerLayoutPanel's children) from the given row and column
            /// </summary>
            /// <param name="row">Row</param>
            /// <param name="column">Column</param>
            /// <returns>Index</returns>
            private int GetIndexFromCell(int row, int column)
            {
                var result = -1;

                if ((row >= 0) && (column >= 0))
                {
                    switch (_panelOrientation)
                    {
                        case Orientation.Horizontal:
                            result = (_cellsPerLine * row) + column;
                            break;
                        case Orientation.Vertical:
                            result = (_cellsPerLine * column) + row;
                            break;
                    }
                }

                return result;
            }

            /// <summary>
            /// Provides the index of the child (in the CustomerLayoutPanel's children) from the given point
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            internal int GetIndexFromPoint(Point p)
            {
                var result = -1;
                if ((p.X > 0.00D) &&
                    (p.X < _panelSize.Width) &&
                    (p.Y > 0.00D) &&
                    (p.Y < _panelSize.Height))
                {
                    GetCellFromPoint(p, out var row, out var column);
                    result = GetIndexFromCell(row, column);
                }

                return result;
            }

            /// <summary>
            /// Provides the row and column of the child based on its index in the CustomerLayoutPanel.Children
            /// </summary>
            /// <param name="index">Index</param>
            /// <param name="row">Row</param>
            /// <param name="column">Column</param>
            private void GetCellFromIndex(int index, out int row, out int column)
            {
                row = column = -1;

                if (index >= 0)
                {
                    switch (_panelOrientation)
                    {
                        case Orientation.Horizontal:
                            row = (int)(index / (double)_cellsPerLine);
                            column = (int)(index % (double)_cellsPerLine);
                            if (_reverseRow)
                            {
                                row = (_finalRows - 1) - row;
                            }
                            break;
                        case Orientation.Vertical:
                            column = (int)(index / (double)_cellsPerLine);
                            row = (int)(index % (double)_cellsPerLine);
                            break;
                    }
                }
            }

            /// <summary>
            /// Provides the row and column of the child based on its location in the CustomerLayoutPanel
            /// </summary>
            /// <param name="p">Location of the child in the parent</param>
            /// <param name="row">Row</param>
            /// <param name="column">Column</param>
            private void GetCellFromPoint(Point p, out int row, out int column)
            {
                row = column = -1;

                if ((p.X < 0.00D) ||
                    (p.X > _panelSize.Width) ||
                    (p.Y < 0.00D) ||
                    (p.Y > _panelSize.Height))
                {
                    return;
                }

                row = (int)(p.Y / _cellSize.Height);
                column = (int)(p.X / _cellSize.Width);
                if (_panelOrientation == Orientation.Horizontal && _reverseRow)
                {
                    row = (_finalRows - 1) - row;
                }
            }

            /// <summary>
            /// Provides the location of the child in the CustomerLayoutPanel based on the given row and column
            /// </summary>
            /// <param name="row">Row</param>
            /// <param name="column">Column</param>
            /// <returns>Location of the child in the panel</returns>
            private Point GetPointFromCell(int row, int column)
            {
                var result = new Point();

                if ((row >= 0) && (column >= 0))
                {
                    result = new Point(_cellSize.Width * column, _cellSize.Height * row);
                }

                return result;
            }

            /// <summary>
            /// Provides the location of the child in the CustomerLayoutPanel based on the given row and column
            /// </summary>
            /// <param name="index">Index</param>
            /// <returns>Location of the child in the panel</returns>
            internal Point GetPointFromIndex(int index)
            {
                var result = new Point();

                if (index >= 0)
                {
                    GetCellFromIndex(index, out var row, out var column);
                    result = GetPointFromCell(row, column);
                }

                return result;
            }

            /// <summary>
            /// Creates a TransformGroup based on the given Translation, Scale and Rotation
            /// </summary>
            /// <param name="transX">Translation in the X-axis</param>
            /// <param name="transY">Translation in the Y-axis</param>
            /// <param name="scaleX">Scale factor in the X-axis</param>
            /// <param name="scaleY">Scale factor in the Y-axis</param>
            /// <returns>TransformGroup</returns>
            internal TransformGroup CreateTransform(double transX, double transY, double scaleX, double scaleY)
            {
                var translation = new TranslateTransform
                {
                    X = transX,
                    Y = transY
                };

                var scale = new ScaleTransform
                {
                    ScaleX = scaleX,
                    ScaleY = scaleY
                };

                var transform = new TransformGroup();
                transform.Children.Add(scale);
                transform.Children.Add(translation);

                return transform;
            }

            /// <summary>
            /// Creates the storyboard for animating a child from its old location to the new location.
            /// The Translation and Scale properties are animated.
            /// </summary>
            /// <param name="element">UIElement for which the storyboard has to be created</param>
            /// <param name="newLocation">New location of the UIElement</param>
            /// <param name="period">Duration of animation</param>
            /// <param name="easing">Easing function</param>
            /// <returns>Storyboard</returns>
            internal Storyboard CreateTransition(UIElement element, Point newLocation, TimeSpan period, EasingFunctionBase easing)
            {
                Duration duration = new Duration(period);

                // Animate X
                DoubleAnimation translateAnimationX = new DoubleAnimation();
                translateAnimationX.To = newLocation.X;
                translateAnimationX.Duration = duration;
                if (easing != null)
                    translateAnimationX.EasingFunction = easing;

                Storyboard.SetTarget(translateAnimationX, element);
                Storyboard.SetTargetProperty(translateAnimationX,
                    new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));

                // Animate Y
                DoubleAnimation translateAnimationY = new DoubleAnimation();
                translateAnimationY.To = newLocation.Y;
                translateAnimationY.Duration = duration;
                if (easing != null)
                    translateAnimationY.EasingFunction = easing;

                Storyboard.SetTarget(translateAnimationY, element);
                Storyboard.SetTargetProperty(translateAnimationY,
                    new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));

                // Animate ScaleX
                DoubleAnimation scaleAnimationX = new DoubleAnimation();
                scaleAnimationX.To = 1.0D;
                scaleAnimationX.Duration = duration;
                if (easing != null)
                    scaleAnimationX.EasingFunction = easing;

                Storyboard.SetTarget(scaleAnimationX, element);
                Storyboard.SetTargetProperty(scaleAnimationX,
                    new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

                // Animate ScaleY
                DoubleAnimation scaleAnimationY = new DoubleAnimation();
                scaleAnimationY.To = 1.0D;
                scaleAnimationY.Duration = duration;
                if (easing != null)
                    scaleAnimationY.EasingFunction = easing;

                Storyboard.SetTarget(scaleAnimationY, element);
                Storyboard.SetTargetProperty(scaleAnimationY,
                    new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));

                Storyboard sb = new Storyboard();
                sb.Duration = duration;
                sb.Children.Add(translateAnimationX);
                sb.Children.Add(translateAnimationY);
                sb.Children.Add(scaleAnimationX);
                sb.Children.Add(scaleAnimationY);

                return sb;
            }

            #endregion
        }

        #endregion


        #region Customer

        /// <summary>
        /// Show the mode button (should used with <see cref="ItemControlDragBehavior"/>)
        /// </summary>
        public bool ShowModeButton
        {
            get => (bool)GetValue(ShowModeButtonProperty);
            set => SetValue(ShowModeButtonProperty, value);
        }

        public static readonly DependencyProperty ShowModeButtonProperty =
            DependencyProperty.Register("ShowModeButton", typeof(bool), typeof(CustomerLayoutPanel), new PropertyMetadata(false));


        public DragOrientation DragOrientation
        {
            get => (DragOrientation)GetValue(DragOrientationProperty);
            set => SetValue(DragOrientationProperty, value);
        }

        public static readonly DependencyProperty DragOrientationProperty =
            DependencyProperty.Register("DragOrientation", typeof(DragOrientation), typeof(CustomerLayoutPanel), new PropertyMetadata(DragOrientation.All));



        public int RowHeight
        {
            get => (int)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(CustomerLayoutPanel), new PropertyMetadata(0));


        public int ColumnWidth
        {
            get => (int)GetValue(ColumnWidthProperty);
            set => SetValue(ColumnWidthProperty, value);
        }

        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.Register("ColumnWidth", typeof(int), typeof(CustomerLayoutPanel), new PropertyMetadata(0));


        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomerLayoutPanel panel)
            {
                var rowHeight = (int)panel.GetValue(RowHeightProperty);
                panel.SetValue(HeightProperty, (double)((int)e.NewValue * rowHeight));
            }
        }


        #endregion

    }

    public class CustomPanelAdorner : System.Windows.Documents.Adorner
    {
        private readonly VisualCollection _visualCollection;
        private readonly ToggleButton _btn;
        private readonly Canvas _canvas;

        public EventHandler OnDragModeStart;

        public CustomPanelAdorner(UIElement adornedElement, ItemControlDragBehavior listBoxDragBehavior) : base(adornedElement)
        {
            _visualCollection = new VisualCollection(this);

            _btn = new ToggleButton();
            var btnStyle = (Style)FindResource("DraggingMode.ToggleButton");
            _btn.SetValue(StyleProperty, btnStyle);

            var binding = new Binding()
            {
                Source = listBoxDragBehavior,
                Path = new PropertyPath("DraggingMode")
            };
            _btn.SetBinding(ToggleButton.IsCheckedProperty, binding);

            _btn.Click += OnButtonClick;

            _canvas = new Canvas();
            _canvas.Children.Add(_btn);
            _visualCollection.Add(_canvas);
        }


        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            OnDragModeStart?.Invoke(null, EventArgs.Empty);
        }

        protected override int VisualChildrenCount => _visualCollection.Count;

        protected override Visual GetVisualChild(int index)
        {
            return _visualCollection[index];
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _canvas.Arrange(new Rect(finalSize));

            var margin = new Thickness(finalSize.Width - _btn.ActualWidth - 30, finalSize.Height - _btn.ActualHeight - 30, 0, 0);
            _btn.Margin = margin;

            return base.ArrangeOverride(finalSize);
        }
    }

    public class ItemDroppedEventArgs : RoutedEventArgs, IItemDroppedEventArgs
    {
        public int PreviousIndex { get; private set; }

        public int CurrentIndex { get; private set; }
        public object DataContext { get; set; }

        public ItemDroppedEventArgs(RoutedEvent routedEvent, int previousIndex, int currentIndex, object dataContext)
            : base(routedEvent)
        {
            PreviousIndex = previousIndex;
            CurrentIndex = currentIndex;
            DataContext = dataContext;
        }

        public object GetSourceDataContext()
        {
            if (Source is FrameworkElement frameworkElement)
            {
                return frameworkElement.DataContext;
            }
            return null;
        }

        public object GetOriginalSourceDataContext()
        {
            if (OriginalSource is FrameworkElement frameworkElement)
            {
                return frameworkElement.DataContext;
            }

            return null;
        }

    }

    public enum DragOrientation
    {
        Horizontal,
        Vertical,
        All
    }

    public interface IRoutedEventArgs
    {
        object Source { get; }

        object OriginalSource { get; }

        object GetSourceDataContext();

        object GetOriginalSourceDataContext();
    }

    public interface IItemDroppedEventArgs : IRoutedEventArgs
    {
        /// <summary>
        /// Index of dragged item
        /// </summary>
        int PreviousIndex { get; }

        /// <summary>
        /// new index when dragging item dropped
        /// </summary>
        int CurrentIndex { get; }

        object DataContext { get; set; }
    }
}
