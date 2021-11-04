using System;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class TextBoxDemoViewModel : ViewModelBase
    {
        private string _hexValue;

        public ushort DecValue { get; set; }

        public string HexValue
        {
            get => _hexValue;
            set
            {
                var input = value;

                if (IsValid(input))
                {
                    if (input.Equals(string.Empty))
                    {
                        _hexValue = "0";
                        input = "0";
                    }
                    try
                    {
                        DecValue = Convert.ToUInt16(input, 16);
                        RaisePropertyChanged(() => DecValue);
                        _hexValue = value;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public TextBoxDemoViewModel()
        {
            HexValue = "12AE";
        }

        private bool IsValid(string text)
        {
            if (text.Equals(string.Empty))
            {
                return true;
            }
            var reg = new System.Text.RegularExpressions.Regex("^[0-9A-Fa-f]*$");
            return reg.IsMatch(text);
        }

    }
}
