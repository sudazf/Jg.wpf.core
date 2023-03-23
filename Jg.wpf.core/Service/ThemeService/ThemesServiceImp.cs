using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Jg.wpf.core.Service.ThemeService
{
    internal class ThemesServiceImp : IThemeService
    {
        public void Apply(bool isLight)
        {
            AddResourceDictionary(Application.Current.Resources.MergedDictionaries,
                isLight
                    ? "/Jg.wpf.controls;component/Themes/Colors/JgColor.Light.xaml"
                    : "/Jg.wpf.controls;component/Themes/Colors/JgColor.Dark.xaml");
        }

        private void AddResourceDictionary(Collection<ResourceDictionary> mergedDictionaries, string resourcePath, UriKind uriKind = UriKind.Relative)
        {
            if (mergedDictionaries != null && !string.IsNullOrEmpty(resourcePath))
            {
                try
                {
                    ResourceDictionary resourceDictionary = new ResourceDictionary
                    {
                        Source = new Uri(resourcePath, uriKind)
                    };
                    if (resourceDictionary.MergedDictionaries.Count > 0)
                    {
                        foreach (var resourceDictionaryMergedDictionary in resourceDictionary.MergedDictionaries)
                        {
                            AddResourceDictionary(mergedDictionaries, resourceDictionaryMergedDictionary.Source.LocalPath);
                        }
                    }

                    mergedDictionaries.Add(resourceDictionary);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
