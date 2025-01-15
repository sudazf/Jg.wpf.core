using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using Jg.wpf.core.Extensions.Types;

namespace Jg.wpf.controls.Customer
{
    public class MultiSelectComboBox : ListBox
    {
        private const string PART_Popup = "PART_Popup";
        private const string PART_CheckBoxAll = "PART_CheckBoxAll";
        
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(MultiSelectComboBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty MaxDropDownHeightProperty
            = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(MultiSelectComboBox),
                new PropertyMetadata(SystemParameters.PrimaryScreenHeight / 3));

        public static readonly DependencyProperty SelectAllContentProperty =
            DependencyProperty.Register("SelectAllContent", typeof(object), typeof(MultiSelectComboBox),
                new PropertyMetadata("SelectAll"));

        public static readonly DependencyProperty IsSelectAllActiveProperty =
            DependencyProperty.Register("IsSelectAllActive", typeof(bool), typeof(MultiSelectComboBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty DelimiterProperty =
            DependencyProperty.Register("Delimiter", typeof(string), typeof(MultiSelectComboBox),
                new PropertyMetadata(";"));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox),
                new PropertyMetadata(string.Empty, OnTextChanged));

        private bool _ignoreTextValueChanged;
        private MultiSelectComboBoxItem _multiSelectComboBoxItem;
        private Popup _popup;

        public string ItemDisplayPath { get; set; }
        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        [Bindable(true)]
        [Category("Layout")]
        [TypeConverter(typeof(LengthConverter))]
        public double MaxDropDownHeight
        {
            get => (double)GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }
        public object SelectAllContent
        {
            get => GetValue(SelectAllContentProperty);
            set => SetValue(SelectAllContentProperty, value);
        }
        public bool IsSelectAllActive
        {
            get => (bool)GetValue(IsSelectAllActiveProperty);
            set => SetValue(IsSelectAllActiveProperty, value);
        }
        public string Delimiter
        {
            get => (string)GetValue(DelimiterProperty);
            set => SetValue(DelimiterProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "ItemsSource")
            {
                var value = e.NewValue;
                var judge = value is IEnumerable<ISelectable>;
                if (!judge)
                {
                    MessageBox.Show("ItemsSource must bind to types implemented from ISelectable.", "System info", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            base.OnPropertyChanged(e);
        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiSelectComboBoxItem;
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiSelectComboBoxItem();
        }
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            UpdateText();
            base.OnSelectionChanged(e);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = GetTemplateChild(PART_Popup) as Popup;
            if (_popup != null)
                _popup.MouseDown += (s, e) => e.Handled = true; //防止点击内部自动关闭 Popup

            _multiSelectComboBoxItem = GetTemplateChild(PART_CheckBoxAll) as MultiSelectComboBoxItem;
            if (_multiSelectComboBoxItem != null)
            {
                _multiSelectComboBoxItem.Selected += OnMultiSelectComboBoxItem_Selected;
                _multiSelectComboBoxItem.Unselected += OnMultiSelectComboBoxItem_Unselected;
            }
        }

        protected virtual void UpdateText()
        {
            if (_ignoreTextValueChanged) 
                return;

            var newValue = string.Join(Delimiter, SelectedItems.Cast<object>().Select(GetItemDisplayValue));
            if (string.IsNullOrWhiteSpace(Text) || !Text.Equals(newValue))
            {
                _ignoreTextValueChanged = true;
                _multiSelectComboBoxItem?.SetCurrentValue(IsSelectedProperty, SelectedItems.Count == Items.Count);
                SetCurrentValue(TextProperty, newValue);
                _ignoreTextValueChanged = false;
            }
        }
        protected object GetItemDisplayValue(object item)
        {
            if (string.IsNullOrWhiteSpace(ItemDisplayPath))
            {
                var property = item.GetType().GetProperty("Content");
                if (property != null)
                    return property.GetValue(item, null);
            }
            else
            {
                var nameParts = ItemDisplayPath.Split('.');
                if (nameParts.Length == 1)
                {
                    var property = item.GetType().GetProperty(ItemDisplayPath);
                    if (property != null)
                        return property.GetValue(item, null);
                }
            }

            return item;
        }

        private void OnMultiSelectComboBoxItem_Unselected(object sender, RoutedEventArgs e)
        {
            if (_ignoreTextValueChanged) return;
            _ignoreTextValueChanged = true;
            UnselectAll();
            _ignoreTextValueChanged = false;
            UpdateText();
        }
        private void OnMultiSelectComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (_ignoreTextValueChanged) return;
            _ignoreTextValueChanged = true;
            SelectAll();
            _ignoreTextValueChanged = false;
            UpdateText();
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }

    public class MultiSelectComboBoxItem : ListBoxItem
    {
        private const string PART_Indicator = "PART_Indicator";
        private const string PART_Border = "PART_Border";
        
        private Border _indicator;
        private Border _border;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = GetTemplateChild(PART_Border) as Border;
            if (_border != null)
                _border.MouseDown += (s, e) => e.Handled = true; //防止点击内部自动关闭 Popup

            _indicator = GetTemplateChild(PART_Indicator) as Border;
            if (_indicator != null)
                _indicator.MouseDown += (s, e) => e.Handled = true; //防止点击内部自动关闭 Popup
        }
    }
}
