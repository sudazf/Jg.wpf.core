using Jg.wpf.core.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using Jg.wpf.core.Extensions.Types;

namespace Jg.wpf.controls.Customer.SelectAll
{
    public class SelectAllListBox : ListBox
    {
        public static readonly RoutedEvent OnSelectAllChangedEvent = EventManager.RegisterRoutedEvent(
            name: "OnSelectAllChanged",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(SelectAllListBox));

        public event RoutedEventHandler OnSelectAllChanged
        {
            add => AddHandler(OnSelectAllChangedEvent, value);
            remove => RemoveHandler(OnSelectAllChangedEvent, value);
        }

        public bool? IsSelectAll
        {
            get => (bool?)GetValue(IsSelectAllProperty);
            set => SetValue(IsSelectAllProperty, value);
        }

        public static readonly DependencyProperty IsSelectAllProperty =
            DependencyProperty.Register("IsSelectAll", typeof(bool?), typeof(SelectAllListBox), new PropertyMetadata(default(bool?)));

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(SelectAllListBox), new PropertyMetadata(true));

        public FrameworkElement ColumnHeader
        {
            get => (FrameworkElement)GetValue(ColumnHeaderProperty);
            set => SetValue(ColumnHeaderProperty, value);
        }

        public static readonly DependencyProperty ColumnHeaderProperty =
            DependencyProperty.Register("ColumnHeader", typeof(FrameworkElement), typeof(SelectAllListBox), new PropertyMetadata(null));


        public JCommand SelectAllCommand { get; }

        public SelectAllListBox()
        {
            SelectAllCommand = new JCommand("SelectAllCommand", OnSelectAll, CanSelectAll);
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (oldValue is IEnumerable<ISelectable> oldSelectors)
            {
                ReleaseSelectionChangedHandle(oldSelectors);
            }
            if (newValue is IEnumerable<ISelectable> selectors)
            {
                SubscribeSelectionChangedHandle(selectors);
            }

            SelectAllCommand.RaiseCanExecuteChanged();
            OnCustomSelectionChanged(this, EventArgs.Empty);

            base.OnItemsSourceChanged(oldValue, newValue);
        }

        private void OnSelectAll(object obj)
        {
            if (ItemsSource is IEnumerable<ISelectable> selectors)
            {
                foreach (var selector in selectors)
                {
                    selector.OnSelectedChanged -= OnCustomSelectionChanged;

                    if (IsSelectAll != null)
                    {
                        selector.IsSelected = IsSelectAll.Value;
                    }

                    selector.OnSelectedChanged += OnCustomSelectionChanged;
                }

                RaiseEvent(new SelectAllEventArgs(OnSelectAllChangedEvent, this, IsSelectAll != null && IsSelectAll.Value));
            }
        }
        private bool CanSelectAll(object arg)
        {
            if (ItemsSource is IEnumerable<ISelectable> selectors)
            {
                return selectors.Any();
            }

            return false;
        }

        private void ReleaseSelectionChangedHandle(IEnumerable<ISelectable> items)
        {
            foreach (var item in items)
            {
                if (item is ISelectable selector)
                {
                    selector.OnSelectedChanged -= OnCustomSelectionChanged;
                }
            }
        }
        private void SubscribeSelectionChangedHandle(IEnumerable<ISelectable> items)
        {
            foreach (var item in items)
            {
                if (item is ISelectable selector)
                {
                    selector.OnSelectedChanged += OnCustomSelectionChanged;
                }
            }
        }
        private void OnCustomSelectionChanged(object sender, EventArgs e)
        {
            var actualStatus = CheckSelectAllState();

            IsSelectAll = actualStatus;
        }

        private bool? CheckSelectAllState()
        {
            if (ItemsSource is IEnumerable<ISelectable> selectors)
            {
                var total = selectors.Count();
                var selectedCount = selectors.Count(c => c.IsSelected);

                if (selectedCount == 0)
                {
                    return false;
                }

                if (selectedCount < total)
                {
                    return default;
                }

                if (selectedCount == total)
                {
                    return true;
                }

                return default;
            }

            return false;
        }
    }
}
