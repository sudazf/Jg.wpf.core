using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Jg.wpf.core.Utility;

namespace Jg.wpf.controls.Converter
{
    public class LocalizationConverterExtension : MarkupExtension, IValueConverter, IMultiValueConverter
    {
        [ThreadStatic]
        private static LocalizationConverterExtension _converter;
        public static LocalizationConverterExtension Converter => _converter ?? (_converter = new LocalizationConverterExtension());
        private static string Ignore = "Ignore:";
        private static string StringFormat = "StringFormat:";
        public static readonly string Bilingual = "Bilingual:";
        public static readonly string AddPrefix = "AddPrefix:";
        public static readonly string NewLine = "NewLine_";

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(new[] { value }, targetType, parameter, culture);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length >= 1)
            {
                if (values.Length == 2 && values[1] is bool)
                {
                    if (bool.TryParse(values[1].ToString(), out var isLocalized) && !isLocalized)
                    {
                        return values[0];
                    }
                }

                var valueString = string.Format(culture, "{0}", values[0]);
                if (parameter is string parameterString && !string.IsNullOrEmpty(valueString))
                {
                    if (parameterString.StartsWith(Ignore))
                    {
                        string needIgnore = parameterString.Remove(0, Ignore.Length);
                        if (!string.IsNullOrEmpty(needIgnore) && string.Equals(valueString, needIgnore))
                        {
                            return string.Empty;
                        }
                    }
                    else if (parameterString.StartsWith(StringFormat))
                    {
                        string formatString = parameterString.Remove(0, StringFormat.Length);
                        if (!string.IsNullOrEmpty(formatString))
                        {
                            try
                            {
                                return string.Format(formatString, TranslateHelper.Translate(valueString));
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }
                    else if (parameterString.StartsWith(Bilingual))
                    {
                        string language = parameterString.Remove(0, Bilingual.Length);
                        if (!string.IsNullOrEmpty(language))
                        {
                            return TranslateHelper.Translate(valueString, false);
                        }
                    }
                    else if (parameterString.StartsWith(NewLine) && valueString.StartsWith(NewLine))
                    {
                        string translatedString = TranslateHelper.Translate(valueString);
                        if (!string.IsNullOrEmpty(translatedString) && translatedString.Contains(" "))
                        {
                            return translatedString.Replace(" ", Environment.NewLine);
                        }
                    }
                    else if (parameterString.StartsWith(AddPrefix))
                    {
                        string addPrefix = parameterString.Remove(0, AddPrefix.Length);
                        if (!string.IsNullOrEmpty(addPrefix))
                        {
                            string translatedString = TranslateHelper.Translate(addPrefix + valueString);
                            if (!string.IsNullOrEmpty(translatedString) && !string.Equals(addPrefix + valueString, translatedString))
                                return translatedString;
                        }
                    }
                }
                return TranslateHelper.Translate(valueString);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
