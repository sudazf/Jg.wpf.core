using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Jg.wpf.app.Controls
{
    /// <summary>
    /// NumericTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxDemo : UserControl
    {
        public TextBoxDemo()
        {
            InitializeComponent();
        }
    }

    public class NumericValidationRule : ValidationRule
    {
        public NumericValidationWrapper Wrapper { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false,"value can not be null!");
            }

            var result = Wrapper?.ValidateRange(value.ToString());
            if (result == false)
            {
                return new ValidationResult(false, "value out of range!");
            }

            return ValidationResult.ValidResult;
        }
    }

    public class NumericValidationWrapper : DependencyObject
    {
        private Regex _numericRegex;

        public double Min
        {
            get => (double)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(double), typeof(NumericValidationWrapper), new PropertyMetadata(-99999d));


        public double Max
        {
            get => (double)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(NumericValidationWrapper), new PropertyMetadata(99999d));


        public int Decimals
        {
            get => (int)GetValue(DecimalsProperty);
            set => SetValue(DecimalsProperty, value);
        }

        public static readonly DependencyProperty DecimalsProperty =
            DependencyProperty.Register("Decimals", typeof(int), typeof(NumericValidationWrapper), new PropertyMetadata(3, OnDecimalsChanged));


        public NumericValidationWrapper()
        {
            UpdateRegex();
        }

        public bool ValidateRegex(string value)
        {
           return _numericRegex.IsMatch(value);
        }

        public bool ValidateRange(string value)
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
                    if (res > Max || res < Min)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        private static void OnDecimalsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericValidationWrapper wrapper)
            {
                wrapper.UpdateRegex();
            }
        }

        private void UpdateRegex()
        {
            var pattern = "^[-]{1}[0-9]*[.]{0,1}[0-9]{0," + Decimals + "}$|^[.][0-9]{0," + Decimals + "}$|^[0-9]*[.]{0,1}[0-9]{0," + Decimals + "}$";
            _numericRegex = new Regex(pattern);
        }

        
    }

    public class MyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value != null)
                {
                    var currentValue = double.Parse(value.ToString());
                    if (currentValue > 1000)
                    {
                        return new ValidationResult(false, "Value must <= 1000");
                    }
                }
                else
                {
                    return new ValidationResult(false, "Value must be set");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ValidationResult(false, e.Message);
            }


            return ValidationResult.ValidResult;
        }
    }
}
