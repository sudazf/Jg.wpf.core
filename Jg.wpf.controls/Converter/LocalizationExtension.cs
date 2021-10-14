using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Jg.wpf.core.Utility;

namespace Jg.wpf.controls.Converter
{
    public class LocalizationExtension : MarkupExtension
    {
        private DependencyObject _uiElement;
        private DependencyProperty _targetProperty;

        [ThreadStatic]
        private static LocalizationConverter _converter;


        public string Name { get; set; }

        public string Path { get; set; }

        public string ElementName { get; set; }

        public object Source { get; set; }

        public bool TemplateBinding { get; set; }

        public bool SelfBinding { get; set; }


        public bool IgnoreWhiteSpace { get; set; } = true;


        public LocalizationExtension()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new LocalizationConverter();
            }

            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            _uiElement = target.TargetObject as DependencyObject;
            if (_uiElement == null)
            {
                return this;
            }
            _targetProperty = target.TargetProperty as DependencyProperty;
            if (_targetProperty == null)
            {
                return this;
            }
            if (DesignTimeHelper.IsInDesignMode)
            {
                if (String.IsNullOrEmpty(Path))
                {
                    return TranslateHelper.Translate(Name);
                }
                var binding = new Binding
                {
                    Path = new PropertyPath(Path),
                    Mode = BindingMode.OneWay,
                    Converter = _converter,
                };
                if (Source != null)
                {
                    binding.Source = Source;
                }
                if (TemplateBinding)
                {
                    binding.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
                }
                if (SelfBinding)
                {
                    binding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
                }
                if (!string.IsNullOrEmpty(ElementName))
                {
                    binding.ElementName = ElementName;
                }

                return binding.ProvideValue(serviceProvider);
            }
            {

                if (String.IsNullOrEmpty(Path))
                {
                    return TranslateHelper.Translate(Name, IgnoreWhiteSpace);
                }
                var binding = new Binding
                {
                    Path = new PropertyPath(Path),
                    Mode = BindingMode.OneWay,
                    Converter = new LocalizationConverter(),
                };
                if (Source != null)
                {
                    binding.Source = Source;
                }
                if (TemplateBinding)
                {
                    binding.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
                }
                if (SelfBinding)
                {
                    binding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
                }
                if (!string.IsNullOrEmpty(ElementName))
                {
                    binding.ElementName = ElementName;
                }

                return binding.ProvideValue(serviceProvider);
            }
        }

        public void SetLocalizationBinding(FrameworkElement targetElement, DependencyProperty targetProperty)
        {
            var localizationBinding = new Binding
            {
                Path = new PropertyPath(Path),
                Mode = BindingMode.OneWay,
                Source = targetElement.DataContext,
                Converter = _converter
            };

            targetElement.SetBinding(targetProperty, localizationBinding);
        }


        private class LocalizationConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return TranslateHelper.Translate($"{value}");
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class DesignTimeHelper
    {
        private static bool? _isInDesignMode;

        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode =
                        (bool)
                        DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty,
                            typeof(FrameworkElement)).Metadata.DefaultValue;
                }
                return _isInDesignMode.Value;
            }
        }
    }
}
