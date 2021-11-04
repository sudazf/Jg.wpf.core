using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jg.wpf.controls.Customer
{
    public class HexTextBox : TextBox
    {
        public HexTextBox()
        {
            PreviewTextInput += OnPreviewTextInput;
            GotFocus += OnCustomGotFocus;
            LostFocus += OnCustomLostFocus;
            DataObject.AddPastingHandler(this, PasteHandle);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 &&
                 e.KeyboardDevice.Modifiers != ModifierKeys.Shift) ||
                (e.Key >= Key.D0 && e.Key <= Key.D9 &&
                 e.KeyboardDevice.Modifiers != ModifierKeys.Shift) ||
                (e.Key >= Key.A && e.Key <= Key.Z) ||
                e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right ||
                e.Key == Key.Enter || e.Key == Key.Decimal ||
                e.Key == Key.End || e.Key == Key.Home || e.Key == Key.Tab)
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

            e.Handled = !IsHex(replaceOrInsert) || !Is16Bit(replaceOrInsert);
        }

        private void OnCustomGotFocus(object sender, RoutedEventArgs e)
        {
            SelectAll();
        }
        private void OnCustomLostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void PasteHandle(object sender, DataObjectPastingEventArgs e)
        {
            var copy = Clipboard.GetText();
            if (!IsHex(copy))
            {
                e.CancelCommand();
            }
        }
        private bool IsHex(string text)
        {
            var valueAsString = text.Replace(" -", string.Empty).ToUpper();

            var reg = new System.Text.RegularExpressions.Regex("^[0-9A-Fa-f]*$");
            var res = reg.IsMatch(valueAsString);
            return res;
        }

        private bool Is16Bit(string text)
        {
            try
            {
                Convert.ToUInt16(text, 16);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

    }
}
