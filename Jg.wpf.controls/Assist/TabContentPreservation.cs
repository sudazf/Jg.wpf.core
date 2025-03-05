using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace Jg.wpf.controls.Assist
{
    public class TabContentPreservation
    {
        #region IsContentPreserved
        public static readonly DependencyProperty IsContentPreservedProperty = DependencyProperty.RegisterAttached(
          "IsContentPreserved", typeof(bool), typeof(TabContentPreservation), new PropertyMetadata(OnIsContentPreservedChanged));

        public static bool GetIsContentPreserved(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsContentPreservedProperty);
        }

        public static void SetIsContentPreserved(DependencyObject obj, bool value)
        {
            obj.SetValue(IsContentPreservedProperty, value);
        }

        private static void OnIsContentPreservedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var tc = obj as TabControl;
            if (tc == null)
            {
                throw new ArgumentException(obj + @" is not TabControl", @"obj");
            }

            if ((bool)e.NewValue)
            {
                var manager = new TabContentManager(tc);
                manager.Start();
                SetTabContentManager(tc, manager);
            }
            else
            {
                var manager = GetTabContentManager(tc);
                if (manager != null)
                {
                    manager.Dispose();
                    SetTabContentManager(tc, DependencyProperty.UnsetValue);
                }
            }
        }
        #endregion IsContentPreserved

        #region TabContentManager
        private static readonly DependencyProperty TabContentManagerProperty = DependencyProperty.RegisterAttached(
          "TabContentManager", typeof(TabContentManager), typeof(TabContentPreservation), new PropertyMetadata(null));

        private static TabContentManager GetTabContentManager(DependencyObject obj)
        {
            return (TabContentManager)obj.GetValue(TabContentManagerProperty);
        }

        private static void SetTabContentManager(DependencyObject obj, object value)
        {
            obj.SetValue(TabContentManagerProperty, value);
        }
        #endregion TabContentManager

        /// <summary>
        /// TabContentManager
        /// </summary>
        private class TabContentManager : IDisposable
        {
            private readonly TabControl _tabControl;
            private DataTemplate _contentTemplate;
            private readonly Dictionary<object, ContentControl> _realizedMap = new Dictionary<object, ContentControl>();

            public TabContentManager(TabControl tabControl)
            {
                _tabControl = tabControl;
            }

            public void Start()
            {
                _tabControl.DataContextChanged += HandleTabDataContextChanged;
            }

            public void Dispose()
            {
                var coll = (INotifyCollectionChanged)_tabControl.Items;
                coll.CollectionChanged -= HandleDataItemCollectionChanged;
                _tabControl.SelectionChanged -= HandleTabSelectionChanged;
                _tabControl.DataContextChanged -= HandleTabDataContextChanged;

                ClearRealizedContent();
            }

            #region Event Handlers
            private void HandleTabDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                var tc = (TabControl)sender;

                tc.SelectionChanged -= HandleTabSelectionChanged;
                var coll = (INotifyCollectionChanged)tc.Items;
                coll.CollectionChanged -= HandleDataItemCollectionChanged;

                if (e.NewValue != null)
                {
                    if (tc.Items.Count > 0)
                    {
                        ////There's Items already, which means the TabControl already has UI controls, we stop here
                        var firstTab = tc.Items[0] as DependencyObject;
                        if (firstTab != null)
                        {
                            throw new InvalidOperationException(string.Format("Content type of {0} is already preserved", tc.Items[0]));
                        }
                    }

                    _contentTemplate = tc.ContentTemplate;
                    tc.ContentTemplate = null;

                    tc.SelectionChanged += HandleTabSelectionChanged;
                    coll.CollectionChanged += HandleDataItemCollectionChanged;
                }
            }

            private ContentControl GetRealizedContent(object item)
            {
                ContentControl contentControl;
                if (_realizedMap.TryGetValue(item, out contentControl))
                {
                    return contentControl;
                }

                return null;
            }

            private void SetRealizedContent(object item, ContentControl contentControl)
            {
                _realizedMap.Add(item, contentControl);
            }

            private void RemoveRealizedContent(object item)
            {
                ContentControl contentControl;
                if (_realizedMap.TryGetValue(item, out contentControl))
                {
                    BindingOperations.ClearAllBindings(contentControl);
                    _realizedMap.Remove(item);
                }
            }

            private void ClearRealizedContent()
            {
                foreach (var contentControl in _realizedMap.Values)
                {
                    BindingOperations.ClearAllBindings(contentControl);
                }

                _realizedMap.Clear();
            }

            private void HandleTabSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                var tc = (TabControl)sender;

                if (e.AddedItems.Count > 0)
                {
                    var dataItem = e.AddedItems[0];
                    var tabItem = (TabItem)tc.ItemContainerGenerator.ContainerFromItem(dataItem);
                    if (tabItem != null)
                    {
                        var contentControl = GetRealizedContent(dataItem);
                        if (contentControl == null)
                        {
                            var template = _contentTemplate;
                            if (template != null)
                            {
                                contentControl = new ContentControl
                                {
                                    DataContext = dataItem,
                                    ContentTemplate = template,
                                    ContentTemplateSelector = tc.ContentTemplateSelector
                                };

                                contentControl.SetBinding(ContentControl.ContentProperty, new Binding());

                                SetRealizedContent(dataItem, contentControl);
                                tabItem.Dispatcher.BeginInvoke((Action)(() => tabItem.Content = contentControl));
                            }
                        }
                    }
                }
            }
            private void HandleDataItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    var items = (ItemCollection)sender;
                    if (items.Count == 0)
                    {
                        ClearRealizedContent();
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var removedItem in e.OldItems)
                    {
                        RemoveRealizedContent(removedItem);
                    }
                }
            }
            #endregion Event Handlers
        }
    }
}
