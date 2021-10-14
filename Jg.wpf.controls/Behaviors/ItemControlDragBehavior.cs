using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Jg.wpf.controls.Customer.LayoutPanel;

namespace Jg.wpf.controls.Behaviors
{
    public class ItemControlDragBehavior : Behavior<ItemsControl>
    {
        private FrameworkElement _dragItem;
        private int _currentTouchId = -1;
        private bool _attachedObjectLoaded;
        private CustomPanelAdorner _customPanelAdorner;
        private bool _mouseDragActived;

        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable", typeof(bool), typeof(ItemControlDragBehavior),
                new FrameworkPropertyMetadata(true));
        public bool Enable
        {
            get => (bool)GetValue(EnableProperty);
            set => SetValue(EnableProperty, value);
        }

        public static readonly DependencyProperty SupportMouseDragProperty =
            DependencyProperty.RegisterAttached("SupportMouseDrag", typeof(bool), typeof(ItemControlDragBehavior),
                new FrameworkPropertyMetadata(false));

        public bool SupportMouseDrag
        {
            get => (bool)GetValue(SupportMouseDragProperty);
            set => SetValue(SupportMouseDragProperty, value);
        }

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

        private bool _mouseInCustomerLayoutPanel = false;
        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
                AssociatedObject.MouseLeave -= OnMouseLeave;
                AssociatedObject.PreviewMouseMove -= OnItemsControlPreviewMouseMove;

                AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
                AssociatedObject.MouseLeave += OnMouseLeave;
                AssociatedObject.PreviewMouseMove += OnItemsControlPreviewMouseMove;

                CustomerLayoutPanel panel = FindChild<CustomerLayoutPanel>(AssociatedObject);
                var adorerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);

                if (panel != null && panel.ShowModeButton)
                {
                    _customPanelAdorner = new CustomPanelAdorner(AssociatedObject);
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
            SupportMouseDrag = !SupportMouseDrag;
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
            if (AssociatedObject != null &&
                AssociatedObject.Items != null && _attachedObjectLoaded)
            {
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
            ItemsControl itemsControl = (ItemsControl)sender;
            CustomerLayoutPanel panel = FindChild<CustomerLayoutPanel>(itemsControl);

            if (_mouseInCustomerLayoutPanel == false)
            {
                return;
            }

            if (SupportMouseDrag)
            {
                if (_mouseDragActived)
                {
                    _mouseDragActived = false;

                    if (_dragItem != null && itemsControl != null)
                    {
                        if (panel != null)
                        {
                            Point point = e.GetPosition(_dragItem);
                            Point pointParent = e.GetPosition(panel);
                            panel.EndDrag(_dragItem, point, pointParent);
                        }
                    }
                    _dragItem = null;
                }
                else
                {
                    if (itemsControl != null)
                    {
                        if (panel != null)
                        {
                            _dragItem = itemsControl.ContainerFromElement((DependencyObject)e.OriginalSource) as FrameworkElement;
                            if (_dragItem != null)
                            {
                                _mouseDragActived = true;
                                ScrollViewer scrollViewer = FindChild<ScrollViewer>(itemsControl);

                                Point point = e.GetPosition(_dragItem);
                                Point pointParent = e.GetPosition(panel);
                                panel.BeginDrag(_dragItem, point);
                            }
                        }
                    }
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

            ItemsControl itemsControl = (ItemsControl)sender;
            if (_dragItem != null && itemsControl != null)
            {
                CustomerLayoutPanel panel = FindChild<CustomerLayoutPanel>(itemsControl);
                if (panel != null)
                {
                    Point point = e.GetPosition(_dragItem);
                    Point pointParent = e.GetPosition(panel);

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
