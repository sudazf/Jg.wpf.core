using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jg.wpf.controls.Customer.NumericTextBox
{
    public class NumericTextBox : TextBox
    {
        private Regex _numericRegex;

        public double Min
        {
            get => (double)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(double), typeof(NumericTextBox), new PropertyMetadata(-99999d));


        public double Max
        {
            get => (double)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(NumericTextBox), new PropertyMetadata(99999d));


        public int Decimals
        {
            get => (int)GetValue(DecimalsProperty);
            set => SetValue(DecimalsProperty, value);
        }

        public static readonly DependencyProperty DecimalsProperty =
            DependencyProperty.Register("Decimals", typeof(int), typeof(NumericTextBox), new PropertyMetadata(3, OnDecimalsChanged));


        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(NumericTextBox), new PropertyMetadata(""));


        private static void OnDecimalsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericTextBox numeric)
            {
                numeric.UpdateRegex();
            }
        }

        public NumericTextBox()
        {
            UpdateRegex();
            PreviewTextInput += OnPreviewTextInput;
            LostFocus += OnCustomLostFocus;
            DataObject.AddPastingHandler(this, PasteHandle);
        }

        private void OnCustomLostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Text, out var res))
            {
                if (res < Min)
                {
                    Text = Min.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            string replaceOrInsert;
            if (textBox.SelectionLength > 0)
            {
                replaceOrInsert = textBox.Text.Replace(textBox.SelectedText, e.Text);
            }
            else
            {
                replaceOrInsert = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            }
            e.Handled = !_numericRegex.IsMatch(replaceOrInsert) || !ValidateRange(replaceOrInsert);
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 &&
                 e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
                ||
                (e.Key >= Key.D0 && e.Key <= Key.D9 &&
                 e.KeyboardDevice.Modifiers != ModifierKeys.Shift) ||
                e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right ||
                e.Key == Key.Enter || e.Key == Key.Decimal ||
                e.Key == Key.OemPeriod || e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key == Key.End
                || e.Key == Key.Home || e.Key == Key.Tab || e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && (e.Key == Key.C || e.Key == Key.V))
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = true;
            }
        }
        private void PasteHandle(object sender, DataObjectPastingEventArgs e)
        {
            var copy = Clipboard.GetText();
            if (!_numericRegex.IsMatch(copy) || !ValidateRange(copy))
            {
                e.CancelCommand();
            }
        }

        private bool ValidateRange(string value)
        {
            bool result = true;

            if (string.IsNullOrEmpty(value))
            {
                result = false;
            }
            else
            {
                if (double.TryParse(value, out var res))
                {
                    if (res > Max)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        private void UpdateRegex()
        {
            var pattern = "^[-]{1}[0-9]*[.]{0,1}[0-9]{0," + Decimals + "}$|^[.][0-9]{0," + Decimals + "}$|^[0-9]*[.]{0,1}[0-9]{0," + Decimals + "}$";
            _numericRegex = new Regex(pattern);
        }
    }
}
