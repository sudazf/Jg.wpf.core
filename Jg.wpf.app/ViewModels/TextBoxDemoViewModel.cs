using System;
using System.Collections.Generic;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class TextBoxDemoViewModel : ViewModelBase
    {
        private string _hexValue;
        private int _selectBit;

        public int SelectBit
        {
            get => _selectBit;
            set
            {
                _selectBit = value;
                HexValue = "0";
                RaisePropertyChanged(()=> SelectBit);
                RaisePropertyChanged(() => HexValue);
            }
        }
        public List<int> Bits { get; }
        public string DecValue { get; set; }
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
                        switch (SelectBit)
                        {
                            case 16:
                                DecValue = Convert.ToUInt16(input, 16).ToString();
                                break;
                            case 32:
                                DecValue = Convert.ToUInt32(input, 16).ToString();
                                break;
                            case 64:
                                DecValue = Convert.ToUInt64(input, 16).ToString();
                                break;
                            default:
                                DecValue = Convert.ToUInt16(input, 16).ToString();
                                break;
                        }
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
            Bits = new List<int>
            {
                16,32,64
            };

            SelectBit = Bits[0];
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
