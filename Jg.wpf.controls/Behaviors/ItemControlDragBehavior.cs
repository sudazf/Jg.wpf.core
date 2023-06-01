using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Jg.wpf.controls.Customer.LayoutPanel;
using Microsoft.Xaml.Behaviors;

namespace Jg.wpf.controls.Behaviors
{
    public class ItemControlDragBehavior : Behavior<ItemsControl>
    {
        #region Fields

        private FrameworkElement _dragItem;
        private int _currentTouchId = -1;
        private bool _attachedObjectLoaded;
        private CustomPanelAdorner _customPanelAdorner;
        private bool _mouseInCustomerLayoutPanel;
        private bool _mouseDragActived;

        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable", typeof(bool), typeof(ItemControlDragBehavior),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty DraggingModeProperty =
            DependencyProperty.RegisterAttached("DraggingMode", typeof(bool), typeof(ItemControlDragBehavior),
                new FrameworkPropertyMetadata(false));

        public bool Enable
        {
            get => (bool)GetValue(EnableProperty);
            set => SetValue(EnableProperty, value);
        }

        public bool DraggingMode
        {
            get => (bool)GetValue(DraggingModeProperty);
            set => SetValue(DraggingModeProperty, value);
        }
        #endregion

        #region Overrides

        protected override void OnAttached()
        {
            // Subscribe to the Loaded event

            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded += OnAssociatedObjectLoaded;
                AssociatedObject.Unloaded += OnAssociatedObjectUnLoaded;

            }
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                AssociatedObject.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
                AssociatedObject.MouseLeave -= OnMouseLeave;
                AssociatedObject.PreviewMouseMove -= OnItemsControlPreviewMouseMove;

                AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
                AssociatedObject.MouseLeave += OnMouseLeave;
                AssociatedObject.PreviewMouseMove += OnItemsControlPreviewMouseMove;

                var panel = FindChild<CustomerLayoutPanel>(AssociatedObject);
                var adorerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);

                if (panel != null && panel.ShowModeButton)
                {
                    _customPanelAdorner = new CustomPanelAdorner(AssociatedObject,this);
                    _customPanelAdorner.OnDragModeStart += OnDragModeChanged;
                    if (adorerLayer != null)
                    {
                        var adorers = adorerLayer.GetAdorners(AssociatedObject);
                        if (adorers?.Length > 0)
                        {
                            adorerLayer.Remove(adorers[0]);
                        }
                        adorerLayer.Add(_customPanelAdorner);
                    }
                    panel.MouseEnter -= Panel_MouseEnter;
                    panel.MouseLeave -= Panel_MouseLeave;

                    panel.MouseEnter += Panel_MouseEnter;
                    panel.MouseLeave += Panel_MouseLeave;
                }
            }
            AssociatedObject?.UpdateLayout();
        }

        private void OnDragModeChanged(object sender, EventArgs e)
        {
        }

        private void Panel_MouseLeave(object sender, MouseEventArgs e)
        {
            _mouseInCustomerLayoutPanel = false;
        }

        private void Panel_MouseEnter(object sender, MouseEventArgs e)
        {
            _mouseInCustomerLayoutPanel = true;
        }

        private void OnAssociatedObjectUnLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject != null && _attachedObjectLoaded)
            {
                AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                AssociatedObject.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
                AssociatedObject.MouseLeave -= OnMouseLeave;
                AssociatedObject.PreviewMouseMove -= OnItemsControlPreviewMouseMove;
                _attachedObjectLoaded = false;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
                AssociatedObject.Unloaded -= OnAssociatedObjectUnLoaded;
            }
        }

        #endregion

        #region Event Handlers

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            OnPreviewMouseLeftButtonUp(sender, e);
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var device = e.StylusDevice;
            if (device != null && device.TabletDevice != null && device.TabletDevice.Type == TabletDeviceType.Touch)
            {
                if (_dragItem != null && _currentTouchId == -1)
                {
                    e.Handled = true;
                    _dragItem = null;
                }

                return;
            }

            var itemsControl = (ItemsControl)sender;
            var panel = FindChild<CustomerLayoutPanel>(itemsControl);

            if (_mouseInCustomerLayoutPanel == false)
            {
                return;
            }

            if (DraggingMode)
            {
                if (!_mouseDragActived)
                {
                    if (itemsControl != null)
                    {
                        if (panel != null)
                        {
                            _dragItem = itemsControl.ContainerFromElement((DependencyObject)e.OriginalSource) as FrameworkElement;
                            if (_dragItem != null)
                            {
                                _mouseDragActived = true;

                                var point = e.GetPosition(_dragItem);
                                panel.BeginDrag(_dragItem, point);
                            }
                        }
                    }
                }

                e.Handled = true;
            }
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            var device = e.StylusDevice;
            if (device != null && device.TabletDevice != null && device.TabletDevice.Type == TabletDeviceType.Touch)
            {
                if (_dragItem != null && _currentTouchId == -1)
                {
                    e.Handled = true;
                    _dragItem = null;
                }

                return;
            }
            var itemsControl = (ItemsControl)sender;
            var panel = FindChild<CustomerLayoutPanel>(itemsControl);

            if (_mouseInCustomerLayoutPanel == false)
            {
                return;
            }

            if (DraggingMode)
            {
                if (_mouseDragActived)
                {
                    _mouseDragActived = false;

                    if (_dragItem != null && itemsControl != null)
                    {
                        if (panel != null)
                        {
                            var point = e.GetPosition(_dragItem);
                            var pointParent = e.GetPosition(panel);
                            panel.EndDrag(_dragItem, point, pointParent);
                        }
                    }
                    _dragItem = null;
                }

                e.Handled = true;
            }
        }

        private void OnItemsControlPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var device = e.StylusDevice;
            if (device != null && device.TabletDevice != null && device.TabletDevice.Type == TabletDeviceType.Touch)
            {
                return;
            }

            var itemsControl = (ItemsControl)sender;
            if (_dragItem != null && itemsControl != null)
            {
                var panel = FindChild<CustomerLayoutPanel>(itemsControl);
                if (panel != null)
                {
                    var point = e.GetPosition(_dragItem);
                    var pointParent = e.GetPosition(panel);

                    panel.DragMove(_dragItem, point, pointParent, AssociatedObject);
                }
            }
        }


        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        #endregion
    }
}
