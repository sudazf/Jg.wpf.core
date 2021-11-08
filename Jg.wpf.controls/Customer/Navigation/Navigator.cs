using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Jg.wpf.controls.Customer.Navigation
{
    public class Navigator : Selector
    {
        private List<NavigatorItem> _items;

        public event EventHandler<string> OnNavigatedTo;

        public FrameworkElement Content
        {
            get => (FrameworkElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(Navigator), new PropertyMetadata(null));


        public DataTemplateSelector DataTemplateSelector    
        {
            get => (DataTemplateSelector)GetValue(DataTemplateSelectorProperty);
            set => SetValue(DataTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty DataTemplateSelectorProperty =
            DependencyProperty.Register("DataTemplateSelector", typeof(DataTemplateSelector), typeof(Navigator), new PropertyMetadata(null));

        public Navigator()
        {
            Loaded += OnControlLoaded;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            if (_items == null)
            {
                _items = new List<NavigatorItem>();
                foreach (var group in this.Items)
                {
                    if (group is NavigatorGroup ng)
                    {
                        foreach (var ngItem in ng.Items)
                        {
                            if (ngItem is NavigatorItem ni)
                            {
                                ni.OnNavigated += OnNavigated;
                                _items.Add(ni);
                            }
                        }
                    }
                }
            }
        }

        private void OnNavigated(object sender, string name)
        {
            foreach (var navigatorItem in _items)
            {
                navigatorItem.IsSelected = false;
            }

            OnNavigatedTo?.Invoke(this, name);
        }
    }
}
